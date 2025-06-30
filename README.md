# X2Pdf - Conversor de Imagens para PDF

API simples para converter múltiplas imagens em um único arquivo PDF.

```mermaid
flowchart TD
    Client["HTTP Client\n(curl, PowerShell)"]:::client
    Client --> API

    subgraph "Docker Container (Port 8080)"
        style DockerContainer container
        API["Kestrel HTTP Listener\n(.NET 9 Minimal API)"]:::api
        Endpoint["Minimal API Endpoint Handler\nPOST /api/convert/images-to-pdf"]:::api
        Service["Conversion Service\nImage-to-PDF processing"]:::logic
        Library["PDF Generation Library\n(PdfSharpCore)"]:::lib
        Swagger["Swagger UI"]:::lib

        API --> Endpoint
        Endpoint --> Service
        Service --> Library
        Endpoint --> Swagger
        Service --> Endpoint
        Endpoint --> Client
    end

    subgraph "Project Files"
        csproj["X2Pdf.csproj"]:::lib
        sln["X2Pdf.sln"]:::lib
        readme["README.md"]:::lib
        dockerfile["Dockerfile"]:::lib
        appjson["appsettings.json"]:::lib
        devjson["appsettings.Development.json"]:::lib
        launch["Properties/launchSettings.json"]:::lib
    end

    click API "https://github.com/gabrielvesal/x2pdf/blob/main/Program.cs"
    click Endpoint "https://github.com/gabrielvesal/x2pdf/blob/main/Program.cs"
    click Service "https://github.com/gabrielvesal/x2pdf/blob/main/Program.cs"
    click Swagger "https://github.com/gabrielvesal/x2pdf/blob/main/Program.cs"
    click csproj "https://github.com/gabrielvesal/x2pdf/blob/main/X2Pdf.csproj"
    click sln "https://github.com/gabrielvesal/x2pdf/blob/main/X2Pdf.sln"
    click readme "https://github.com/gabrielvesal/x2pdf/blob/main/README.md"
    click dockerfile "https://github.com/gabrielvesal/x2pdf/tree/main/Dockerfile"
    click appjson "https://github.com/gabrielvesal/x2pdf/blob/main/appsettings.json"
    click devjson "https://github.com/gabrielvesal/x2pdf/blob/main/appsettings.Development.json"
    click launch "https://github.com/gabrielvesal/x2pdf/blob/main/Properties/launchSettings.json"

    classDef client fill:#D6E9FF,stroke:#3399FF,color:#000
    classDef api fill:#DFF0D8,stroke:#3C763D,color:#000
    classDef logic fill:#FCF8E3,stroke:#8A6D3B,color:#000
    classDef lib fill:#F2F2F2,stroke:#AAA,color:#000
    classDef container stroke:#999,stroke-dasharray:5 5
```

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
### Executar usando a imagem Docker .tar (sem código-fonte)

[Baixe o arquivo x2pdf-latest.tar da release ou do repositório.](https://github.com/GabrielVesal/X2Pdf/releases/tag/v1.0.0)

Abra o terminal e navegue até a pasta onde o .tar está salvo (exemplo Windows):

```
cd C:\Users\gabriel\Downloads
```

Importe a imagem para o Docker local:

```
docker load -i x2pdf-latest.tar
```

Execute o container expondo a porta 8080:

```
docker run -p 8080:8080 x2pdf:latest
```

Acesse a interface Swagger no navegador:

```
http://localhost:8080/swagger/index.html
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
Após o container iniciar, acesse a interface Swagger da API no navegador:

http://localhost:8080/swagger
