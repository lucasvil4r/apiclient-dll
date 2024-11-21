# ApiClient-DLL

Uma biblioteca .NET projetada para facilitar integração com APIs assíncrona no DataFlex.

## Recursos

- Suporte para chamadas HTTP simplificadas (GET).
- Uso de bibliotecas confiáveis como **RestSharp** para gerenciar requisições.
- Compatibilidade com o framework .NET 4.8.
- Registro para interoperabilidade COM, permitindo integração em aplicativos que suportem bibliotecas COM.
- Token de autenticação (Bearer Token) pré-definido.

## Requisitos

- **.NET Framework 4.8**.
- Dependências incluídas:
  - [RestSharp](https://www.nuget.org/packages/RestSharp/) (versão 112.1.0).
  - [Microsoft.Bcl.AsyncInterfaces](https://www.nuget.org/packages/Microsoft.Bcl.AsyncInterfaces/).

---

## Como Usar

### Clone o Repositório
```bash
git clone https://github.com/lucasvil4r/apiclient-dll.git
```

## Configure a Variável de Ambiente no Windows
- Crie uma variável de ambiente chamada ApiClient, apontando para o caminho do executável da aplicação.

## Parâmetros Necessários
Ao executar o aplicativo, os seguintes argumentos devem ser fornecidos na ordem indicada:

- BaseAddress: URL base da API.
- Endpoint: Endpoint da API que será chamado.
- Token: Token de autenticação (Bearer Token).
- TempFilePath: Caminho para o arquivo onde o resultado da requisição será gravado.

## Exemplo de Execução

```bash
ApiClient.exe "https://api.exemplo.com" "/v1/endpoint" "seu_token_aqui" "C:\caminho\para\arquivo.json"
```

##  Fluxo de Operação
**Servidor**: o código executa os seguintes passos:

- Realiza uma chamada GET para o endpoint especificado, utilizando o token fornecido.
- Grava a resposta JSON em um arquivo cujo caminho foi passado como argumento.

```csharp
public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Consultando API...");
        if (args.Length >= 4)
        {
            string baseAddress = args[0];
            string endpoint = args[1];
            string token = args[2];
            string tempFilePath = args[3];

            string json = RequestGetAsyncApi(baseAddress, endpoint, token);
            GetApiResponseFile(tempFilePath, json);
        }
    }
}
```

**Exemplo Cliente**: A aplicação cliente executa o programa console, aguardando que a resposta da API seja gravada no arquivo especificado. Em seguida, lê o conteúdo do arquivo para processá-lo.

```dataflex
Function fRequestRunExecutableApi String llbaseAddress String llendpoint String lltoken Returns String
    String sTemp sFileResponse sRequestRunExecutableApi sBuffer sRet
    Integer iChOut iChIn
    
    Get_Environment "TEMP" to sTemp
    Get_Environment "ApiClient" to sRequestRunExecutableApi // Variavel de ambiente apontando para executavel console
    
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
