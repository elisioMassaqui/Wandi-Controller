using UnityEngine;
using UnityEngine.UI;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System;
using System.Reflection;
using System.Text;
using System.Linq;
using System.IO;
using TMPro;

public class CodeEditor : MonoBehaviour
{
    public InputField codeInputField;
    public Button compileButton;
    public TMP_Text outputText;

    private string defaultCode = @"
using UnityEngine;

public class Script : MonoBehaviour
{
    void Start()
    {
        Debug.Log(""Hello, World!"");
    }
}";

    void Awake()
    {
        // Inicializar o InputField com o código padrão
        if (codeInputField != null)
        {
            codeInputField.text = defaultCode;
        }
    }

    void Start()
    {
        if (compileButton != null)
        {
            compileButton.onClick.AddListener(CompileAndRunCode);
        }
    }

    void CompileAndRunCode()
    {
        string code = codeInputField.text;
        string result = CompileAndExecute(code);
        outputText.text = result;
    }

    string CompileAndExecute(string code)
    {
        // Configurar a sintaxe da árvore de código
        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code);

        // Adicionar referências
        var references = AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => !a.IsDynamic && !string.IsNullOrEmpty(a.Location))
            .Select(a => MetadataReference.CreateFromFile(a.Location))
            .ToList();

        // Adicionar referências específicas do Unity
        references.Add(MetadataReference.CreateFromFile(typeof(UnityEngine.Debug).Assembly.Location));
        references.Add(MetadataReference.CreateFromFile(typeof(UnityEngine.MonoBehaviour).Assembly.Location));

        CSharpCompilation compilation = CSharpCompilation.Create(
            "DynamicAssembly",
            new[] { syntaxTree },
            references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        // Compilar o código em memória
        using (var ms = new MemoryStream())
        {
            EmitResult result = compilation.Emit(ms);

            if (!result.Success)
            {
                StringBuilder errors = new StringBuilder();
                foreach (Diagnostic diagnostic in result.Diagnostics)
                {
                    errors.AppendLine($"{diagnostic.Id}: {diagnostic.GetMessage()}");
                }
                return errors.ToString();
            }

            // Carregar o assembly em memória
            ms.Seek(0, SeekOrigin.Begin);
            Assembly assembly = Assembly.Load(ms.ToArray());

            // Procurar a classe Script e executar o método Start
            Type scriptType = assembly.GetType("Script");
            if (scriptType != null)
            {
                GameObject go = new GameObject("DynamicScript");
                var scriptComponent = go.AddComponent(scriptType);
                MethodInfo startMethod = scriptType.GetMethod("Start", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (startMethod != null)
                {
                    startMethod.Invoke(scriptComponent, null);
                    return "Código executado com sucesso!";
                }
                else
                {
                    return "Método Start não encontrado!";
                }
            }
            else
            {
                return "Classe Script não encontrada!";
            }
        }
    }
}
