# Script simples para testar a API
$imagePath = "C:\Users\gatal\Downloads\1744570923858.jpg"
$apiUrl = "https://localhost:7167/api/convert/image-to-pdf"
$outputPath = "C:\Users\gatal\Downloads\converted.pdf"

Write-Host "Testando conversão..." -ForegroundColor Green
Write-Host "Imagem: $imagePath" -ForegroundColor Yellow
Write-Host "API: $apiUrl" -ForegroundColor Yellow

# Verificar se a imagem existe
if (-not (Test-Path $imagePath)) {
    Write-Host "ERRO: Imagem não encontrada em $imagePath" -ForegroundColor Red
    exit 1
}

try {
    $form = @{
        file = Get-Item $imagePath
    }
    
    Write-Host "Enviando requisição..." -ForegroundColor Cyan
    
    $response = Invoke-RestMethod -Uri $apiUrl -Method Post -Form $form -OutFile $outputPath
    
    Write-Host "SUCESSO! PDF criado em: $outputPath" -ForegroundColor Green
    Write-Host "Tamanho do arquivo: $((Get-Item $outputPath).Length) bytes" -ForegroundColor Green
    
} catch {
    Write-Host "ERRO: $($_.Exception.Message)" -ForegroundColor Red
    if ($_.Exception.Response) {
        $reader = New-Object System.IO.StreamReader($_.Exception.Response.GetResponseStream())
        $responseBody = $reader.ReadToEnd()
        Write-Host "Resposta do servidor: $responseBody" -ForegroundColor Red
    }
} 