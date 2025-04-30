using Xadrez.Models.Pecas;

namespace Xadrez.Models.Estrategias;

/// <summary>
/// Representa a estratégia de movimento de uma peça no tabuleiro de xadrez.
/// </summary>
/// <param name="peca">Representa a peça que está sendo movida.</param>
/// <param name="tabuleiro">Representa o tabuleiro onde a peça está sendo movida.</param>
public class EstrategiaMovimentoCavalo(ICavalo peca, ITabuleiro tabuleiro): EstrategiaMovimento(peca, tabuleiro)
{
    /// <summary>
    /// Peça que está sendo movida.
    /// </summary>
    protected IPeca Peca { get; } = peca;

    /// <summary>
    /// Tabuleiro onde a peça está sendo movida.
    /// </summary>
    protected ITabuleiro Tabuleiro { get; } = tabuleiro;


    /// <summary>
    /// Calcula as casas possíveis para a peça de acordo com a estratégia e o estado do tabuleiro.
    /// </summary>
    /// <returns>Lista de casas possíveis para movimentação.</returns>
    public override List<Movimento> GeraMovimentos()
    {
        var movimentos = new List<Movimento>();
        var casa = Tabuleiro.ObtemCasaPeca(Peca);
        if (casa is not null)
        {
            AdicionarMovimentos(casa, -2, -1, movimentos, 1);
            AdicionarMovimentos(casa, -2, 1, movimentos, 1);
            AdicionarMovimentos(casa, -1, -2, movimentos, 1);
            AdicionarMovimentos(casa, -1, 2, movimentos, 1);
            AdicionarMovimentos(casa, 1, -2, movimentos, 1);
            AdicionarMovimentos(casa, 1, 2, movimentos, 1);
            AdicionarMovimentos(casa, 2, -1, movimentos, 1);
            AdicionarMovimentos(casa, 2, 1, movimentos, 1);
        }

        return movimentos;
    }
}
