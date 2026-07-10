# ApiClient DLL

Biblioteca e executavel para realizar chamadas HTTP `GET` autenticadas com Bearer Token, com foco em integracao com DataFlex e outros clientes que consumam COM ou executem processos externos.

## Visao geral

A solucao foi organizada em tres projetos:

- `ApiClient.Core`: biblioteca compartilhada com a regra de requisicao HTTP e escrita de resposta em arquivo.
- `ApiClient`: DLL `.NET Framework 4.8` exposta via COM Interop.
- `ConsoleAppRequestApiClient`: executavel `.NET 8` que recebe parametros por linha de comando, chama a API e grava a resposta em arquivo.

Essa separacao evita duplicacao de codigo. Novas funcionalidades devem entrar primeiro no `ApiClient.Core`; depois, os projetos externos apenas expoem essa funcionalidade para o tipo de consumidor desejado.

## Requisitos

- Visual Studio 2022 ou superior.
- .NET Framework 4.8 Developer Pack.
- .NET SDK 8 ou superior.
- Windows, caso use COM Interop ou registro da DLL.

## Como compilar

Abra a solucao:

```powershell
apiclient-dll.sln
```

Ou compile por linha de comando:

```powershell
dotnet build .\apiclient-dll.sln
```

Para registrar a DLL COM, compile o projeto `ApiClient` com `RegisterForComInterop` habilitado. O projeto esta configurado como `x86`, adequado para consumidores COM 32-bit.

## Uso via executavel

O executavel recebe os argumentos nesta ordem:

1. `BaseAddress`: URL base da API.
2. `Endpoint`: endpoint chamado via `GET`.
3. `Token`: token de autenticacao Bearer.
4. `TempFilePath`: caminho do arquivo onde a resposta sera gravada.

Exemplo:

```powershell
ConsoleAppRequestApiClient.exe "https://api.exemplo.com" "/v1/clientes" "seu_token" "C:\Temp\resposta.json"
```

Fluxo:

1. O executavel monta a requisicao.
2. Envia `Accept: application/json`.
3. Envia `Authorization: Bearer {token}` quando o token for informado.
4. Executa `GET`.
5. Grava a resposta ou erro no arquivo informado.

## Uso via COM

A DLL expoe a classe COM:

- `ProgId`: `ApiClient`
- Interface: `IApiClient`
- Metodo: `RequestGetAsyncApi(string baseAddress, string endpoint, string token)`

O metodo retorna uma `string` com o conteudo da resposta ou uma mensagem de erro.

### Registro da DLL

Execute os scripts na raiz do repositorio **como Administrador**:

- **Registrar:** `register.bat`
- **Remover registro:** `unregister.bat`

Os scripts apontam para `ApiClient\bin\Release\ApiClient.dll`. Tambem e possivel registrar manualmente:

```cmd
%windir%\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe ApiClient.dll /codebase /tlb
```

Para remover o registro manualmente:

```cmd
%windir%\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe ApiClient.dll /unregister
```

> **Importante:** todas as DLLs dependentes devem estar na mesma pasta que `ApiClient.dll`:
>
> - `ApiClient.Core.dll`
> - `RestSharp.dll`
> - `System.Text.Json.dll`
> - `System.Text.Encodings.Web.dll`
> - `System.Buffers.dll`
> - `System.Memory.dll`
> - `System.Numerics.Vectors.dll`
> - `System.Runtime.CompilerServices.Unsafe.dll`
> - `System.Threading.Tasks.Extensions.dll`
> - `System.IO.Pipelines.dll`
> - `Microsoft.Bcl.AsyncInterfaces.dll`

### Exemplo DataFlex via COM

```dataflex
Object oApiClient is a cComAutomationObject
    Set psProgId to "ApiClient"
End_Object

Function fRequestComApi String llBaseAddress String llEndpoint String llToken Returns String
    String sResult
    Variant vResult

    Send CreateComObject of oApiClient
    Get ComRequestGetAsyncApi of oApiClient llBaseAddress llEndpoint llToken to vResult
    Move vResult to sResult
    Send ReleaseComObject of oApiClient

    Function_Return sResult
End_Function
```

## Exemplo DataFlex

```dataflex
Function fRequestRunExecutableApi String llbaseAddress String llendpoint String lltoken Returns String
    String sTemp sFileResponse sRequestRunExecutableApi sBuffer sRet
    Integer iChOut iChIn

    Get_Environment "TEMP" to sTemp
    Get_Environment "ApiClient" to sRequestRunExecutableApi

    Move (sTemp + "\" + (String(DateGetMillisecond(CurrentDateTime()))) + ".json") to sFileResponse

    Move (Seq_New_Channel()) to iChOut
    Direct_Output channel iChOut sFileResponse
    Close_Output channel iChOut
    Send Seq_Release_Channel iChOut

    Runprogram Shell Background sRequestRunExecutableApi (llbaseAddress * llendpoint * lltoken * sFileResponse)
    Sleep 2

    Move (Seq_New_Channel()) to iChIn
    Direct_Input channel iChIn sFileResponse
    While (not(SeqEof))
        Readln channel iChIn sBuffer
        Move (sRet + sBuffer) to sRet
    Loop
    Close_Input iChIn
    Send Seq_Release_Channel iChIn

    Function_Return sRet
End_Function
```

## Documentacao adicional

Veja [docs/architecture.md](docs/architecture.md) para detalhes da arquitetura, responsabilidades dos projetos e orientacoes para novas funcionalidades.
