using Xadrez.Models.Pecas;

namespace Xadrez.Models.Estrategias;

/// <summary>
/// Interface que define a estratégia de movimento de uma peça no tabuleiro de xadrez.
/// </summary>
public interface IEstrategiaMovimento
{
    /// <summary>
    /// Peça que está sendo movida.
    /// </summary>
    IPeca Peca { get; }

    /// <summary>
    /// Tabuleiro onde a peça está sendo movida.
    /// </summary>
    ITabuleiro Tabuleiro { get; }

    /// <summary>
    /// Calcula as casas possíveis para a peça de acordo com a estratégia e o estado do tabuleiro.
    /// </summary>
    /// <param name="tabuleiro">Representa o tabuleiro onde a peça está sendo movida.</param>
    /// <returns>Lista de movimentos possíveis para a peça.</returns>
    List<Movimento> ObterMovimentosPossiveis();

    /// <summary>
    /// Valida os movimentos possíveis para a peça, garantindo que não resultem em xeque ao rei do jogador.
    /// </summary>
    /// <param name="movimentos">Lista de movimentos a serem validados. Cada movimento é uma instância da classe Movimento.</param>
    void ValidaMovimentos(List<Movimento> movimentos);

    /// <summary>
    /// Gera os movimentos possíveis para a peça, considerando as regras do xadrez e o estado atual do tabuleiro.
    /// </summary>
    /// <returns>Uma lista de movimentos possíveis para a peça. Cada movimento é uma instância da classe Movimento.</returns>
    List<Movimento> GeraMovimentos();
}
