@echo off
echo Registrando ApiClient.dll...
%windir%\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe "%~dp0ApiClient\bin\Release\ApiClient.dll" /codebase /tlb
if %errorlevel% neq 0 (
    echo Falha ao registrar. Execute como Administrador.
    pause
    exit /b 1
)
echo Registro concluido com sucesso.
pause
