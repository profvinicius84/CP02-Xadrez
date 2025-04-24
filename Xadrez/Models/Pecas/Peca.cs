

namespace Xadrez.Models.Pecas;

public class Peca : IPeca
{
    public bool EBranca => throw new NotImplementedException();

    public List<Movimento> MovimentosPossiveis(Tabuleiro tabuleiro)
    {
        var movimentos = new List<Movimento>();

        foreach(var casa in tabuleiro.Casas)
        {
            if(casa.Peca.EBranca != this.EBranca)
            {
                movimentos.Add(new Movimento(this, tabuleiro.ObtemCasaPeca(this), casa, casa.Peca));
            }
        }

        return movimentos;
    }
}