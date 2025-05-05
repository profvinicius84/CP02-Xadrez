using Xadrez.Models.Pecas;

namespace Xadrez.Models.Pecas;

/// <summary>
/// Classe que representa o Rei no jogo de xadrez.
/// </summary>
/// <param name="eBranca">Indica se a peça é branca ou preta.</param>
public class Rei(bool eBranca) : Peca(eBranca), IRei
{
    /// <summary>
    /// Indica se o rei está em cheque ou não.
    /// </summary>
    public bool EmCheque { get; set; }

    /// <summary>
    /// Devolve os movimentos possíveis para o Rei.
    /// </summary>
    /// <param name="tabuleiro">O tabuleiro atual do jogo.</param>
    /// <returns>Uma lista de movimentos possíveis para o Rei.</returns>
    public override List<Movimento> MovimentosPossiveis(Tabuleiro tabuleiro)
    {
        var movimentos = new List<Movimento>();
        var casaAtual = tabuleiro.ObtemCasaPeca(this);

        if (casaAtual == null)
            return movimentos;

        // Movimentos possíveis: 1 casa em qualquer direção
        int[] direcaoLinha = { -1, -1, -1, 0, 0, 1, 1, 1 };
        int[] direcaoColuna = { -1, 0, 1, -1, 1, -1, 0, 1 };

        for (int i = 0; i < 8; i++)
        {
            int novaLinha = casaAtual.Linha + direcaoLinha[i];
            int novaColuna = casaAtual.Coluna + direcaoColuna[i];

            // Verifica se está dentro do tabuleiro
            if (novaLinha >= 0 && novaLinha < 8 && novaColuna >= 0 && novaColuna < 8)
            {
                Casa casaDestino = tabuleiro.Casas.First(c => c.Linha == novaLinha && c.Coluna == novaColuna);

                // Verifica se pode mover para a casa
                if (casaDestino.Peca == null || casaDestino.Peca.EBranca != this.EBranca)
                {
                    // Verifica se não está em xeque
                    if (!tabuleiro.VerificaPerigo(casaDestino, this.EBranca))
                    {
                        movimentos.Add(new Movimento(this, casaAtual, casaDestino, casaDestino.Peca));
                    }
                }
            }
        }

        // Verifica roque pequeno
        if (VerificaRoque(tabuleiro, true))
        {
            Casa casaDestinoRei = tabuleiro.Casas.First(c => c.Linha == casaAtual.Linha && c.Coluna == casaAtual.Coluna + 2);
            movimentos.Add(new Movimento(this, casaAtual, casaDestinoRei, null, true));
        }

        // Verifica roque grande
        if (VerificaRoque(tabuleiro, false))
        {
            Casa casaDestinoRei = tabuleiro.Casas.First(c => c.Linha == casaAtual.Linha && c.Coluna == casaAtual.Coluna - 2);
            movimentos.Add(new Movimento(this, casaAtual, casaDestinoRei, null, true));
        }

        return movimentos;
    }

    /// <summary>
    /// Verifica se o rei pode fazer o roque (pequeno ou grande).
    /// </summary>
    /// <param name="tabuleiro">O tabuleiro atual do jogo.</param>
    /// <param name="roquePequeno">Indica se o roque é pequeno ou grande. O roque pequeno é feito com a torre do lado do rei e o roque grande é feito com a torre do lado da dama.</param>
    /// <returns>Retorna verdadeiro se o roque é possível, caso contrário, retorna falso.</returns>
    public bool VerificaRoque(Tabuleiro tabuleiro, bool roquePequeno = false)
    {
        // Rei não pode ter se movido
        if (this.FoiMovimentada)
            return false;

        // Rei não pode estar em xeque
        if (this.EmCheque)
            return false;

        var casaRei = tabuleiro.ObtemCasaPeca(this);
        if (casaRei == null)
            return false;

        // Verifica torre
        int colunaTorre = roquePequeno ? 7 : 0;
        var casaTorre = tabuleiro.Casas.FirstOrDefault(c => c.Linha == casaRei.Linha && c.Coluna == colunaTorre);

        if (casaTorre?.Peca is not ITorre torre || torre.EBranca != this.EBranca || torre.FoiMovimentada)
            return false;

        // Verifica caminho livre
        int direcao = roquePequeno ? 1 : -1;
        int distancia = roquePequeno ? 2 : 3;

        for (int i = 1; i <= distancia; i++)
        {
            int colunaVerificar = casaRei.Coluna + (i * direcao);
            var casaVerificar = tabuleiro.Casas.First(c => c.Linha == casaRei.Linha && c.Coluna == colunaVerificar);

            if (i < distancia || (!roquePequeno && i == distancia))
            {
                if (casaVerificar.Peca != null)
                    return false;
            }

            if (i <= 2)
            {
                if (tabuleiro.VerificaPerigo(casaVerificar, this.EBranca))
                    return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Executa o roque (pequeno ou grande) no tabuleiro.
    /// </summary>
    /// <param name="tabuleiro">O tabuleiro atual do jogo.</param>
    /// <param name="roquePequeno">Indica se o roque é pequeno ou grande. O roque pequeno é feito com a torre do lado do rei e o roque grande é feito com a torre do lado da dama.</param>
    /// <returns>Retorna o movimento executado pelo roque. O movimento é composto pelo rei e pela torre que se movem ao mesmo tempo.</returns>
    public Movimento ExecutaRoque(Tabuleiro tabuleiro, bool roquePequeno = false)
    {
        var casaRei = tabuleiro.ObtemCasaPeca(this);
        if (casaRei == null)
            throw new InvalidOperationException("Rei não encontrado no tabuleiro");

        // Posições da torre
        int colunaTorreOrigem = roquePequeno ? 7 : 0;
        int colunaTorreDestino = roquePequeno ? 5 : 3;

        var casaTorreOrigem = tabuleiro.Casas.First(c => c.Linha == casaRei.Linha && c.Coluna == colunaTorreOrigem);
        var casaTorreDestino = tabuleiro.Casas.First(c => c.Linha == casaRei.Linha && c.Coluna == colunaTorreDestino);

        if (casaTorreOrigem.Peca is not ITorre torre)
            throw new InvalidOperationException("Torre não encontrada para executar o roque");

        // Posições do rei
        int colunaReiDestino = roquePequeno ? casaRei.Coluna + 2 : casaRei.Coluna - 2;
        var casaReiDestino = tabuleiro.Casas.First(c => c.Linha == casaRei.Linha && c.Coluna == colunaReiDestino);

        // Cria movimento
        var movimentoRoque = new Movimento(this, casaRei, casaReiDestino, null, true);

        // Move rei
        casaRei.Peca = null;
        casaReiDestino.Peca = this;
        this.FoiMovimentada = true;

        // Move torre
        casaTorreOrigem.Peca = null;
        casaTorreDestino.Peca = torre;
        torre.FoiMovimentada = true;

        return movimentoRoque;
    }
} 