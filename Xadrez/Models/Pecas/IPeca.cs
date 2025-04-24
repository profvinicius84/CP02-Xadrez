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
    /// Devolve lista de movimentos possíveis para a peça.
    /// </summary>
    /// <param name="tabuleiro"></param>
    /// <returns></returns>
    List<Movimento> MovimentosPossiveis(Tabuleiro tabuleiro);
}