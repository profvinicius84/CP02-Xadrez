using Xadrez.Models.Estrategias;

namespace Xadrez.Models.Pecas;

/// <summary>
/// Representa uma peça especial e poderosa que não existe no xadrez tradicional.
/// </summary>
/// <param name="eBranca">Indica se a peça é branca ou preta.</param>
public class Hades(bool eBranca) : Peca(eBranca)
{
    public override IEstrategiaMovimento ObtemEstrategiaMovimento(ITabuleiro tabuleiro)
    {
        return new EstrategiaMovimentoPecas(this, tabuleiro, true, true, true, true);
    }


    /// <summary>
    /// Devolve os movimentos possíveis para a peça Hades.
    /// </summary>
    /// <param name="tabuleiro">O tabuleiro atual do jogo.</param>
    /// <returns>Uma lista de movimentos possíveis para a peça Hades.</returns>
    public override List<Movimento> MovimentosPossiveis(ITabuleiro tabuleiro)
    {
        var movimentos = new List<Movimento>();

        for(int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                var casa = tabuleiro.ObtemCasaCoordenadas(i, j);
                if (casa.Peca is null || casa.Peca.EBranca != this.EBranca)
                {
                    movimentos.Add(new Movimento(this, tabuleiro.ObtemCasaPeca(this), casa, casa.Peca));
                }
            }
        }
        return movimentos;
    }
}