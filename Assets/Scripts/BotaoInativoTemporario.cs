using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BotaoInativoTemporario : MonoBehaviour
{
    [Range(0.001f, 10f)] // Especifica um intervalo de 0 a 100
    public float tempoInative = 1f; // Tempo em segundos que os botões ficarão inativos

    Controller controller;

    public void DesativarBotao(Button botao)
    {
        // Desativa o botão quando é clicado
        botao.interactable = false;

        // Inicia uma coroutine para reativar o botão após um período de tempo
        StartCoroutine(ReativarBotao(botao));
    }

    private IEnumerator ReativarBotao(Button botao)
    {
        // Aguarda o tempo especificado
        yield return new WaitForSeconds(tempoInative);

        // Ativa o botão novamente
        botao.interactable = true;
    }
}
