using Xadrez.Models.Pecas;

namespace Xadrez.Models.Estrategias;

/// <summary>
/// Representa a estratégia de movimento de uma peça no tabuleiro de xadrez.
/// </summary>
/// <param name="peca">Representa a peça que está sendo movida.</param>
/// <param name="tabuleiro">Representa o tabuleiro onde a peça está sendo movida.</param>
/// <param name="vertical">Indica se a peça pode se mover verticalmente.</param>
/// <param name="horizontal">Indica se a peça pode se mover horizontalmente.</param>
/// <param name="diagonal">Indica se a peça pode se mover diagonalmente.</param>
/// <param name="permiteRecuo">Indica se a peça pode recuar.</param>
/// <param name="limiteCasas">Indica o número máximo de casas que a peça pode se mover.</param>
public class EstrategiaMovimentoPecas(IPeca peca, ITabuleiro tabuleiro, bool vertical, bool horizontal, bool diagonal, bool permiteRecuo, int limiteCasas = 8): EstrategiaMovimento(peca, tabuleiro)
{
    private readonly bool _vertical = vertical;
    private readonly bool _horizontal = horizontal;
    private readonly bool _diagonal = diagonal;
    private readonly bool _permiteRecuo = permiteRecuo;
    private readonly int _limiteCasas = limiteCasas;

    /// <summary>
    /// Calcula as casas possíveis para a peça de acordo com a estratégia e o estado do tabuleiro.
    /// </summary>
    /// <returns>Lista de casas possíveis para movimentação.</returns>
    public override List<Movimento> GeraMovimentos()
    {
        var movimentos = new List<Movimento>();
        var casa = Tabuleiro.ObtemCasaPeca(Peca);        
        int direcao = Peca.EBranca ? 1 : -1;

        // Movimento vertical
        if (_vertical)
        {
            AdicionarMovimentos(casa, direcao, 0, movimentos, _limiteCasas);
            if (_permiteRecuo)
                AdicionarMovimentos(casa, -direcao, 0, movimentos, _limiteCasas);
        }

        // Movimento horizontal
        if (_horizontal)
        {
            AdicionarMovimentos(casa, 0, 1, movimentos, _limiteCasas);
            AdicionarMovimentos(casa, 0, -1, movimentos, _limiteCasas);
        }

        // Movimento diagonal
        if (_diagonal)
        {
            AdicionarMovimentos(casa, direcao, 1, movimentos, _limiteCasas);
            AdicionarMovimentos(casa, direcao, -1, movimentos, _limiteCasas);

            if (_permiteRecuo)
            {
                AdicionarMovimentos(casa, -direcao, 1, movimentos, _limiteCasas);
                AdicionarMovimentos(casa, -direcao, -1, movimentos, _limiteCasas);
            }
        }
        return movimentos;
    }    
}
