using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TerminalController : MonoBehaviour
{
    public Controller controller;

    public Text displayText; // Referência ao texto que será alterado
    public InputField inputField; // Referência ao campo de entrada

    // Método chamado quando o Input Field é submetido
    public void OnCommandEntered()
    {
        string command = inputField.text;

        // Verifica o comando e altera o texto da UI
        if (command.StartsWith("wandi "))
        {
            string newText = command.Substring(6);
            displayText.text = newText;
        }

        // Limpa o campo de entrada após a execução do comando
        inputField.text = "";
        inputField.ActivateInputField(); // Volta o foco para o campo de entrada
    }

    void Update()
    {
        if (displayText.text == "abdrir porta")
        {
            if (!controller.serialPort.IsOpen)
            {
                controller.OpenPort();
            }
        }
    }
}
