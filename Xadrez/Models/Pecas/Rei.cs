namespace Xadrez.Models.Pecas;

/// <summary>
/// Representa a peça Rei no xadrez.
/// </summary>
/// <param name="eBranca">Indica se a peça é branca ou preta.</param>
public class Rei(bool eBranca) : Peca(eBranca), IRei
{
    /// <summary>
    /// Devolve os movimentos possíveis para o Rei.
    /// </summary>
    /// <param name="tabuleiro">O tabuleiro atual do jogo.</param>
    /// <returns>Uma lista de movimentos possíveis para o cavalo.</returns>
    public override List<Movimento> ObterMovimentosPossiveis(Casa origem, ITabuleiro tabuleiro)
    {
        var movimentos = new List<Movimento>();
        int[,] direcoes = new int[,]
        {
        { -1, -1 }, { -1, 0 }, { -1, 1 },
        {  0, -1 },           {  0, 1 },
        {  1, -1 }, {  1, 0 }, {  1, 1 }
        };

        for (int i = 0; i < direcoes.GetLength(0); i++)
        {
            int novaLinha = origem.Linha + direcoes[i, 0];
            int novaColuna = origem.Coluna + direcoes[i, 1];

            if (tabuleiro.EhPosicaoValida(novaLinha, novaColuna))
            {
                var destino = tabuleiro.ObterCasa(novaLinha, novaColuna);
                if (destino.Peca == null || destino.Peca.Cor != this.Cor)
                {
                    movimentos.Add(new Movimento(this, origem, destino));
                }
            }
        }

        return movimentos;
    }

