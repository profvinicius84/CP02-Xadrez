using Xadrez.Models.Pecas;

namespace Xadrez.Models.Estrategias;

/// <summary>
/// Representa a estratégia de movimento de uma peça no tabuleiro de xadrez.
/// </summary>
/// <param name="peca">Representa a peça que está sendo movida.</param>
/// <param name="tabuleiro">Representa o tabuleiro onde a peça está sendo movida.</param>
public abstract class EstrategiaMovimento(IPeca peca, ITabuleiro tabuleiro): IEstrategiaMovimento
{
    /// <summary>
    /// Peça que está sendo movida.
    /// </summary>
    public IPeca Peca { get; } = peca;

    /// <summary>
    /// Tabuleiro onde a peça está sendo movida.
    /// </summary>
    public ITabuleiro Tabuleiro { get; } = tabuleiro;

    /// <summary>
    /// Calcula as casas possíveis para a peça de acordo com a estratégia e o estado do tabuleiro.
    /// </summary>
    /// <param name="tabuleiro">Representa o tabuleiro onde a peça está sendo movida.</param>
    /// <returns>Lista de movimentos possíveis para a peça.</returns>
    public List<Movimento> ObterMovimentosPossiveis()
    {
        var movimentos = GeraMovimentos();
        return movimentos;
    }

    /// <summary>
    /// Valida os movimentos possíveis para a peça, garantindo que não resultem em xeque ao rei do jogador.
    /// </summary>
    /// <param name="movimentos">Lista de movimentos a serem validados. Cada movimento é uma instância da classe Movimento.</param>
    public void ValidaMovimentos(List<Movimento> movimentos)
    {
        movimentos.RemoveAll(m => !Tabuleiro.ValidaMovimento(m));
    }

    /// <summary>
    /// Gera os movimentos possíveis para a peça, considerando as regras do xadrez e o estado atual do tabuleiro.
    /// </summary>
    /// <returns>Uma lista de movimentos possíveis para a peça. Cada movimento é uma instância da classe Movimento.</returns>
    public abstract List<Movimento> GeraMovimentos();

    /// <summary>
    /// Adiciona movimentos possíveis para a peça com base na direção e no estado do tabuleiro.
    /// </summary>
    /// <param name="casaOrigem">A casa de origem da peça.</param>
    /// <param name="deltaLinha">A mudança na linha (vertical) para o movimento.</param>
    /// <param name="deltaColuna">A mudança na coluna (horizontal) para o movimento.</param>
    /// <param name="movimentos">A lista de movimentos possíveis a serem preenchidos.</param>
    protected virtual void AdicionarMovimentos(Casa casaOrigem, int deltaLinha, int deltaColuna, List<Movimento> movimentos, int limiteCasas)
    {

        int linha = casaOrigem.Linha;
        int coluna = casaOrigem.Coluna;

        for (int i = 0; i < limiteCasas; i++)
        {
            linha += deltaLinha;
            coluna += deltaColuna;

            if (linha < 0 || linha > 7 || coluna < 0 || coluna > 7)
                break;

            var casa = Tabuleiro.ObtemCasaCoordenadas(linha, coluna);
            if (casa is null)
                break;

            if (casa.Peca is null)
            {
                // Casa vazia, pode mover
                movimentos.Add(new(Peca, casaOrigem, casa));
            }
            else
            {
                // Casa ocupada: verificar se pode capturar
                if (casa.Peca.EBranca != Peca.EBranca)
                    movimentos.Add(new(Peca, casaOrigem, casa, casa.Peca));

                break; // Bloqueia o caminho depois da peça
            }
        }
    }
}
