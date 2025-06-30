using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.IO.Image;
using iText.Kernel.Geom;
using iText.Layout.Properties;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Png;
using Path = System.IO.Path;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapPost("/api/convert/images-to-pdf", async (IFormFileCollection files) =>
{
    if (files == null || files.Count == 0)
    {
        return Results.BadRequest("Nenhum arquivo fornecido");
    }

    var allowedTypes = new HashSet<string> { "image/png", "image/jpeg", "image/jpg", "image/gif", "image/bmp" };
    var validFiles = files.Where(f => f.Length > 0 && allowedTypes.Contains(f.ContentType.ToLower())).ToList();

    if (validFiles.Count == 0)
    {
        return Results.BadRequest("Nenhuma imagem válida encontrada. Apenas arquivos PNG, JPEG, GIF, BMP são aceitos");
    }

    try
    {
        var processedImages = await Task.WhenAll(validFiles.Select(async file =>
        {
            using var imageStream = file.OpenReadStream();
            using var image = await Image.LoadAsync(imageStream);

            var pageSize = PageSize.A4;
            var maxWidth = pageSize.GetWidth() - 50;
            var maxHeight = pageSize.GetHeight() - 50;

            var scaleX = maxWidth / image.Width;
            var scaleY = maxHeight / image.Height;
            var scale = Math.Min(scaleX, scaleY);

            var newWidth = (int)(image.Width * scale);
            var newHeight = (int)(image.Height * scale);

            image.Mutate(x => x.Resize(newWidth, newHeight));

            using var tempStream = new MemoryStream();
            await image.SaveAsPngAsync(tempStream);
            
            return new
            {
                ImageData = tempStream.ToArray(),
                Width = newWidth,
                Height = newHeight
            };
        }));

        using var pdfStream = new MemoryStream();
        var writer = new PdfWriter(pdfStream);
        var pdf = new PdfDocument(writer);
        var document = new Document(pdf);

        for (int i = 0; i < processedImages.Length; i++)
        {
            var processedImage = processedImages[i];

            var pdfImage = new iText.Layout.Element.Image(ImageDataFactory.Create(processedImage.ImageData));
            pdfImage.SetWidth(processedImage.Width);
            pdfImage.SetHeight(processedImage.Height);
            pdfImage.SetHorizontalAlignment(HorizontalAlignment.CENTER);

            document.Add(pdfImage);

            if (i < processedImages.Length - 1)
            {
                document.Add(new AreaBreak());
            }
        }

        document.Close();

        var fileName = $"{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
        return Results.File(pdfStream.ToArray(), "application/pdf", fileName);
    }
    catch (Exception ex)
    {
        return Results.BadRequest($"Erro ao converter imagens para PDF: {ex.Message}");
    }
})
.DisableAntiforgery()
.WithName("ConvertImagesToPdf")
.WithOpenApi();

app.Run();
