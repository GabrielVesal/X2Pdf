# 🚀 X2Pdf - API de Conversão de Imagens para PDF

> **Transforme suas imagens em PDFs de forma eficiente! 📄**

Uma Minimal API .NET 9 otimizada que converte múltiplas imagens em um único PDF, utilizando processamento paralelo e técnicas de performance.

## ✨ Características Principais

- 🎯 **Minimal API** - Código limpo e direto ao ponto
- ⚡ **Performance Otimizada** - Processamento paralelo com `Task.WhenAll`
- 🖼️ **Múltiplos Formatos** - PNG, JPEG, GIF, BMP
- 📄 **PDF Único** - Todas as imagens em um documento
- 🐳 **Docker Ready** - Containerização completa
- 📊 **Swagger UI** - Documentação interativa
- 🔧 **Cross-Platform** - Funciona em Windows, Linux, macOS

## 🚀 Otimizações de Performance

### Processamento Paralelo com `Task.WhenAll`

```csharp
// Processamento em paralelo para melhor performance
var processedImages = await Task.WhenAll(validFiles.Select(async file =>
{
    using var imageStream = file.OpenReadStream();
    using var image = Image.FromStream(imageStream, false, false); // Sem validação extra
    
    // Cálculo otimizado de dimensões
    var pageSize = PageSize.A4;
    var maxWidth = pageSize.GetWidth() - 50;
    var maxHeight = pageSize.GetHeight() - 50;
    
    var scaleX = maxWidth / (float)image.Width;
    var scaleY = maxHeight / (float)image.Height;
    var scale = Math.Min(scaleX, scaleY);
    
    // Conversão para bytes
    using var tempStream = new MemoryStream();
    image.Save(tempStream, ImageFormat.Png);
    
    return new ProcessedImage
    {
        ImageData = tempStream.ToArray(),
        Width = image.Width * scale,
        Height = image.Height * scale
    };
}));
```

### 🎯 Técnicas de Otimização Implementadas

1. **HashSet para Validação Rápida**
   ```csharp
   var allowedTypes = new HashSet<string> { "image/png", "image/jpeg", "image/jpg", "image/gif", "image/bmp" };
   ```

2. **Carregamento de Imagem Otimizado**
   ```csharp
   Image.FromStream(imageStream, false, false) // Sem validação extra
   ```

3. **Pré-cálculo de Dimensões**
   ```csharp
   var scale = Math.Min(scaleX, scaleY); // Evita recálculos
   ```

4. **Loop com Índice**
   ```csharp
   for (int i = 0; i < processedImages.Length; i++) // Mais eficiente que foreach
   ```

5. **Gerenciamento de Memória**
   ```csharp
   using var tempStream = new MemoryStream(); // Dispose automático
   ```

## 🛠️ Tecnologias Utilizadas

- **.NET 9** - Framework mais recente
- **iText7** - Biblioteca PDF de alta qualidade
- **System.Drawing.Common** - Processamento de imagens
- **Minimal APIs** - Arquitetura moderna e eficiente
- **Swagger/OpenAPI** - Documentação automática

## 📦 Instalação e Execução

### Pré-requisitos
- .NET 9 SDK
- Docker (opcional)

### Execução Local
```bash
# Clone o repositório
git clone https://github.com/gabrielvesal/X2Pdf.git
cd X2Pdf

# Restaure as dependências
dotnet restore

# Execute a aplicação
dotnet run
```

### Execução com Docker
```bash
# Build da imagem
docker build -t x2pdf .

# Execução do container
docker run -p 5000:5000 x2pdf
```

## 🎯 Como Usar

### Endpoint Principal
```
POST /api/convert/images-to-pdf
```

### Exemplo com cURL
```bash
curl -X POST "https://localhost:5000/api/convert/images-to-pdf" \
  -F "files=@imagem1.png" \
  -F "files=@imagem2.jpg" \
  -F "files=@imagem3.gif" \
  --output output.pdf
```

## 📊 Performance

A API foi otimizada para processar múltiplas imagens de forma eficiente:

- **Processamento paralelo** com `Task.WhenAll`
- **Validação otimizada** usando HashSet
- **Gerenciamento de memória** eficiente
- **Cálculos pré-otimizados** de dimensões

*O tempo de processamento varia conforme o tamanho e quantidade das imagens enviadas.*


## 🧪 Testes

### Swagger UI
Acesse `https://localhost:5000/swagger` para testar a API interativamente.

## 🤝 Contribuição

1. Fork o projeto
2. Crie uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

## 📝 Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.
