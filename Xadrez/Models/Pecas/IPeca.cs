namespace Xadrez.Models.Pecas;

/// <summary>
/// Interface que representa uma peça de xadrez.
/// </summary>
public interface IPeca
{
    /// <summary>
    /// Indica se a peça é uma branca ou não.
    /// </summary>
    bool EBranca { get; }

    /// <summary>
    /// Indica se a peça foi movimentada ou não.
    /// </summary>
    bool FoiMovimentada { set;  get; }

    /// <summary>
    /// Devolve lista de movimentos possíveis para a peça.
    /// </summary>
    /// <param name="tabuleiro">O tabuleiro atual do jogo.</param>
    /// <returns>Uma lista de movimentos possíveis para a peça.</returns>
    List<Movimento> MovimentosPossiveis(Tabuleiro tabuleiro);

    /// <summary>
    /// Devolve o código da peça.
    /// </summary>
    string Codigo { get; }
}