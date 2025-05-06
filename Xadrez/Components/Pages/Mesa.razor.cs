using Xadrez.Models.Pecas;

namespace Xadrez.Components.Pages;

/// <summary>
/// Classe que representa a mesa de xadrez.
/// </summary>
public partial class Mesa
{
    /// <summary>
    /// Representa o jogador 1 (branco).
    /// </summary>
    public Models.Jogador Jogador1 { get; set; }

    /// <summary>
    /// Representa o jogador 2 (preto).
    /// </summary>
    public Models.Jogador Jogador2 { get; set; }

    /// <summary>
    /// Representa a partida de xadrez.
    /// </summary>
    public Models.Partida<Models.Tabuleiro> Partida { get; set; }

    /// <summary>
    /// Construtor da classe Mesa.
    /// </summary>
    public Mesa()
    {
        Jogador1 = new Models.Jogador("Branco", true);
        Jogador2 = new Models.Jogador("Preto", false);
        Partida = new Models.Partida<Models.Tabuleiro>(Jogador1, Jogador2);            
    }

    /// <summary>
    /// Reinicia a partida de xadrez.
    /// </summary>
    public void Reiniciar()
    {
        Partida = new Models.Partida<Models.Tabuleiro>(Jogador1, Jogador2);
    }

    /// <summary>
    /// Desfaz o último movimento realizado na partida de xadrez.
    /// </summary>
    public void Desfazer()
    {            
        if (Partida.Movimentos.Count > 0)
        {
            var movimento = Partida.Movimentos.Pop();
            # region Partida.Tabuleiro.ReverteMovimento(movimento)
            movimento.CasaOrigem.Peca = movimento.Peca;
            movimento.CasaDestino.Peca = null;
            if (movimento.PecaCapturada is not null)
            {
                movimento.CasaDestino.Peca = movimento.PecaCapturada;
                Partida.Tabuleiro.PecasCapturadas.Remove(movimento.CasaDestino.Peca);
                movimento.CasaDestino.Peca.FoiMovimentada = Partida.Movimentos.ToList().Exists(m => m.Peca == movimento.CasaDestino.Peca);
            }
            #endregion
        }
    }

    /// <summary>
    /// Retorna o ícone da peça de xadrez correspondente ao tipo de peça.
    /// </summary>
    /// <param name="peca">Representa a peça de xadrez.</param>
    /// <returns>Retorna uma string com o ícone da peça de xadrez correspondente ao tipo de peça.</returns>
    public string IconePeca(IPeca peca)
    {
        string icone = "fa-solid ";
        switch (peca)
        {
            case IPeao:
                icone += "fa-chess-pawn";
                break;
            case ITorre:
                icone += "fa-chess-rook";
                break;
            case ICavalo:
                icone += "fa-chess-knight";
                break;
            case IBispo:
                icone += "fa-chess-bishop";
                break;
            case IRainha:
                icone += "fa-chess-queen";
                break;
            case IRei:
                icone += "fa-chess-king";
                break;
            default:
                icone += "fa-skull";
                break;
        }
        return icone;
    }
}