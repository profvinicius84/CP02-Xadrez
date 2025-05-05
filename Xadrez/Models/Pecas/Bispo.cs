using Xadrez.Models;

namespace Xadrez.Models.Pecas;

/// <summary>
/// Representa uma peça de xadrez do tipo Bispo.
/// </summary>
/// <param name="eBranca">Indica se a peça é branca ou preta.</param>
public class Bispo(bool eBranca) : Peca(eBranca), IBispo
{
    /// <summary>
    /// Devolve lista de movimentos possíveis para o bispo.
    /// O bispo se move apenas nas diagonais, sem limite de casas, mas não pode pular sobre outras peças.
    /// </summary>
    /// <param name="tabuleiro">O tabuleiro atual do jogo.</param>
    /// <returns>Uma lista de movimentos possíveis para o bispo.</returns>
    public override List<Movimento> MovimentosPossiveis(Tabuleiro tabuleiro)
    {

        var movimentos = new List<Movimento>();
        var casaAtual = tabuleiro.ObtemCasaPeca(this);


        // Se setivermos em cheque, o bispo não pode se mexer
        if (Tabuleiro.VerificaXeque(eBranca)){
            return movimentos;
        }

        // Verificamos se a peça realmente está no tabuleiro (pode ser desnecessário)
        if (casaAtual is null)
            return movimentos;


        // Movimento diagonal superior direita
        for (int i = 1; casaAtual.Linha + i < 8 && casaAtual.Coluna + i < 8; i++)
        {
            var casaDestino = tabuleiro.Casas.First(c => c.Linha == casaAtual.Linha + i && c.Coluna == casaAtual.Coluna + i);
            if (casaDestino.Peca is null)
            {
                movimentos.Add(new Movimento(this, casaAtual, casaDestino));
            }

            else
            {
                if (casaDestino.Peca.EBranca != this.EBranca)
                {
                    movimentos.Add(new Movimento(this, casaAtual, casaDestino, casaDestino.Peca));
                }

                break;
            }
        }

        // Movimento diagonal superior esquerda
        for (int i = 1; casaAtual.Linha + i < 8 && casaAtual.Coluna - i >= 0; i++)
        {
            var casaDestino = tabuleiro.Casas.First(c => c.Linha == casaAtual.Linha + i && c.Coluna == casaAtual.Coluna - i);
            if (casaDestino.Peca is null)
            {
                movimentos.Add(new Movimento(this, casaAtual, casaDestino));
            }

            else
            {
                if (casaDestino.Peca.EBranca != this.EBranca)
                {
                    movimentos.Add(new Movimento(this, casaAtual, casaDestino, casaDestino.Peca));
                }
                break;
            }
        }


        // Movimento diagonal inferior direita
        for (int i = 1; casaAtual.Linha - i >= 0 && casaAtual.Coluna + i < 8; i++)
        {
            var casaDestino = tabuleiro.Casas.First(c => c.Linha == casaAtual.Linha - i && c.Coluna == casaAtual.Coluna + i);
            if (casaDestino.Peca is null)
            {
                movimentos.Add(new Movimento(this, casaAtual, casaDestino));
            }
            else
            {
                if (casaDestino.Peca.EBranca != this.EBranca)
                {
                    movimentos.Add(new Movimento(this, casaAtual, casaDestino, casaDestino.Peca));
                }
                break;
            }
        }

        // Movimento diagonal inferior esquerda
        for (int i = 1; casaAtual.Linha - i >= 0 && casaAtual.Coluna - i >= 0; i++)
        {
            var casaDestino = tabuleiro.Casas.First(c => c.Linha == casaAtual.Linha - i && c.Coluna == casaAtual.Coluna - i);
            if (casaDestino.Peca is null)
            {
                movimentos.Add(new Movimento(this, casaAtual, casaDestino));
            }
            else
            {
                if (casaDestino.Peca.EBranca != this.EBranca)
                {
                    movimentos.Add(new Movimento(this, casaAtual, casaDestino, casaDestino.Peca));
                }
                break;
            }
        }


        return movimentos;
    }
} 