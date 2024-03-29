using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using UnityEngine.UI;
using TMPro;

public class Controller : MonoBehaviour
{
    public SerialPort serialPort;

    public TMP_Dropdown portDropdown;
    public TextMeshProUGUI portaSelecionada;


    public string[] ports;
    public string selectedPort;
    public Image statusPort;

    void Start()
    {

        // Inicialize o SerialPort com as configurações necessárias
        serialPort = new SerialPort();

        // Configurar outras configurações do SerialPort, se necessário
         serialPort.BaudRate = 9600;
         
         statusPort.color = Color.red;
    }

        public void OpenPort()
    {

        // Tentar abrir a porta
        try
        {
            serialPort.PortName = selectedPort;
            serialPort.Open();
            statusPort.color = Color.green;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Erro ao abrir a porta: " + e.Message);
        }
    }

    public void ClosePort()
    {
        // Fechar a porta se estiver aberta
        if (serialPort.IsOpen)
        {
            serialPort.Close();
            statusPort.color = Color.red;
        }
    }



    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            serialPort.Write("A");
        }
        if (Input.GetKey(KeyCode.B))
        {
            serialPort.Write("B");
        }
    }

      // Atualiza a lista de portas e o dropdown
    public void AtualizarPortas()
    {
        // Obter a lista de portas disponíveis
        ports = SerialPort.GetPortNames();

        // Limpar as opções existentes no dropdown
        portDropdown.ClearOptions();

        // Adicionar as portas detectadas como opções no dropdown
        portDropdown.AddOptions(new List<string>(ports));

        // Adicionar um listener para o evento de seleção do dropdown
        portDropdown.onValueChanged.AddListener(OnPortDropdownValueChanged);
    }

    // Manipula a mudança na seleção do dropdown
    private void OnPortDropdownValueChanged(int index)
    {
        selectedPort = portDropdown.options[index].text;
        Debug.Log("Porta selecionada: " + selectedPort);
        portaSelecionada.text = "Porta Selecionada: " + selectedPort;

        // Você pode fazer o que quiser com a porta selecionada, como iniciar a comunicação serial, etc.
    }

    //Soluçao pra primeira opçºao que nºao seleciona
    public void selecionarPorta(int index){

        selectedPort = portDropdown.options[index].text;
        Debug.Log("Porta selecionada: " + selectedPort);
        portaSelecionada.text = "Porta Selecionada: " + selectedPort;
    }

    public void Home(){
        if (serialPort.IsOpen)
        {
         serialPort.Write("X");   
        }
    }


    //Buttons for comunication
    public void J1Min(){
         serialPort.WriteLine("A");   
    }
    public void J1Max(){
         serialPort.WriteLine("B");   
    }

    public void J2Min(){
         serialPort.WriteLine("C");   
    }
    public void J2Max(){
         serialPort.WriteLine("D");   
    }

        public void J3Min(){
         serialPort.WriteLine("E");   
    }
    public void J3Max(){
         serialPort.WriteLine("F");   
    }

        public void J4Min(){
         serialPort.WriteLine("G");   
    }
    public void J4Max(){
         serialPort.WriteLine("H");   
    }

        public void J5Min(){
         serialPort.WriteLine("I");   
    }
    public void J5Max(){
         serialPort.WriteLine("J");   
    }

     public void J6Min(){
         serialPort.WriteLine("K");   
    }
    public void J6Max(){
         serialPort.WriteLine("L");
    }
}
