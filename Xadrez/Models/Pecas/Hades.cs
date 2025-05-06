namespace Xadrez.Models.Pecas;

/// <summary>
/// Representa uma peça especial e poderosa que não existe no xadrez tradicional.
/// </summary>
/// <param name="eBranca">Indica se a peça é branca ou preta.</param>
public class Hades(bool eBranca) : Peca(eBranca)
{
    /// <summary>
    /// Devolve os movimentos possíveis para a peça Hades.
    /// </summary>
    /// <param name="tabuleiro">O tabuleiro atual do jogo.</param>
    /// <returns>Uma lista de movimentos possíveis para a peça Hades.</returns>
    public override List<Movimento> MovimentosPossiveis(Tabuleiro tabuleiro)
    {
        var movimentos = new List<Movimento>();

        foreach (var casa in tabuleiro.Casas)
        {
            if (casa.Peca is null || casa.Peca.EBranca != this.EBranca)
            {
                movimentos.Add(new Movimento(this, tabuleiro.ObtemCasaPeca(this), casa, casa.Peca));
            }
        }

        return movimentos;
    }
}