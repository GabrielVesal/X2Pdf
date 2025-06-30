# X2Pdf - Conversor de Imagens para PDF

API simples para converter múltiplas imagens em um único arquivo PDF.

## Funcionalidades

- Converte imagens PNG, JPEG, GIF e BMP para PDF
- Aceita múltiplas imagens de uma vez
- Cada imagem vira uma página do PDF
- Interface Swagger para testes

## Como Usar

### Executar Localmente
```bash
dotnet restore
dotnet run
```

### Endpoint
```
POST /api/convert/images-to-pdf
```

### Exemplo com PowerShell
```powershell
$images = @("imagem1.png", "imagem2.jpg")
$form = @{}
for ($i = 0; $i -lt $images.Length; $i++) {
    $form["files"] = Get-Item $images[$i]
}

$response = Invoke-RestMethod -Uri "https://localhost:8080/api/convert/images-to-pdf" -Method Post -Form $form
[System.IO.File]::WriteAllBytes("output.pdf", $response)
```

### Exemplo com cURL
```bash
curl -X POST "https://localhost:8080/api/convert/images-to-pdf" \
  -F "files=@imagem1.png" \
  -F "files=@imagem2.jpg" \
  --output output.pdf
```

## Swagger

Acesse [https://x2pdf-01vv.onrender.com/swagger](https://x2pdf-01vv.onrender.com/swagger) para testar a API.

## Tecnologias

- .NET 9
- Minimal APIs

## Docker

```bash
docker build -t x2pdf .
docker run -p 8080:8080 x2pdf
``` 
