using Microsoft.AspNetCore.Components;
using Xadrez.Models;

namespace Xadrez.Components.Shared;

/// <summary>
/// Classe que representa o painel do tabuleiro de xadrez.
/// </summary>
public partial class PainelTabuleiro
{
    /// <summary>
    /// Representa o tabuleiro de xadrez.
    /// </summary>
    [Parameter]
    public Models.Partida<Models.Tabuleiro> Partida { get; set; }

    /// <summary>
    /// Evento que é chamado quando o estado do tabuleiro muda.
    /// </summary>
    [Parameter]
    public EventCallback AoMudarEstado { get; set; }

    /// <summary>
    /// Representa a peça selecionada atualmente no tabuleiro.
    /// </summary>
    public Models.Pecas.IPeca? PecaSelecionada { get; private set; }

    /// <summary>
    /// Representa a lista de movimentos possíveis para a peça selecionada.
    /// </summary>
    public List<Movimento> MovimentosPossiveisPecaSelecionada { get; } = new();

    /// <summary>
    /// Representa a lista de blocos de casa no tabuleiro.
    /// </summary>
    private Dictionary<string, BlocoCasa> BlocosCasa { get; } = new();

    /// <summary>
    /// Evento que é chamado ao selecionar uma casa no tabuleiro.
    /// </summary>
    /// <param name="casa">Representa a casa selecionada no tabuleiro.</param>
    public void CliqueCasa(Models.Casa casa)
    {
        if (PecaSelecionada is null && casa.Peca is not null && Partida.JogadorDaVez.EBranco == casa.Peca.EBranca)
        {
            PecaSelecionada = casa.Peca;
            MovimentosPossiveisPecaSelecionada.AddRange(PecaSelecionada.MovimentosPossiveis(Partida.Tabuleiro));
        }
        else if (casa.Peca is not null && casa.Peca == PecaSelecionada)
        {
            PecaSelecionada = null;
            MovimentosPossiveisPecaSelecionada.Clear();
        }
        else if (MovimentosPossiveisPecaSelecionada is not null && MovimentosPossiveisPecaSelecionada.Exists(m => m.CasaDestino == casa))
        {
            var movimento = MovimentosPossiveisPecaSelecionada.First(m => m.CasaDestino == casa);

            Partida.Tabuleiro.ExecutaMovimento(movimento);           
            PecaSelecionada = null;
            MovimentosPossiveisPecaSelecionada.Clear();
            Partida.Movimentos.Push(movimento);
            AoMudarEstado.InvokeAsync();
        }
        AtualizaCondicaoDisponibilidadeCasas();
    }

    /// <summary>
    /// Atualiza a condição de disponibilidade das casas no tabuleiro.
    /// </summary>
    public void AtualizaCondicaoDisponibilidadeCasas()
    {
        foreach (var casa in Partida.Tabuleiro.Casas)
        {
            BlocosCasa[casa.Codigo].CondicaoDisponibilidade = "";
            if (PecaSelecionada is not null && casa.Peca == PecaSelecionada)
                BlocosCasa[casa.Codigo].CondicaoDisponibilidade = "selecionada";
            else if (MovimentosPossiveisPecaSelecionada is not null)
            {
                if (MovimentosPossiveisPecaSelecionada.Exists(m => m.CasaDestino == casa && m.PecaCapturada is not null))
                    BlocosCasa[casa.Codigo].CondicaoDisponibilidade = "perigo";
                else if (MovimentosPossiveisPecaSelecionada.Exists(m => m.CasaDestino == casa))
                    BlocosCasa[casa.Codigo].CondicaoDisponibilidade = "disponivel";
            }
        }
    }    
}
