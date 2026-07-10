@echo off
echo Removendo registro de ApiClient.dll...
%windir%\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe "%~dp0ApiClient\bin\Release\ApiClient.dll" /unregister
if %errorlevel% neq 0 (
    echo Falha ao remover registro. Execute como Administrador.
    pause
    exit /b 1
)
echo Registro removido com sucesso.
pause
