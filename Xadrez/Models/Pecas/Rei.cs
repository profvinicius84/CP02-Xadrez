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

        // Direções possíveis para o Rei se mover: horizontais, verticais e diagonais (1 casa em cada direção)
        int[] direcaoLinha = { -1, -1, -1, 0, 0, 1, 1, 1 };
        int[] direcaoColuna = { -1, 0, 1, -1, 1, -1, 0, 1 };

        for (int i = 0; i < 8; i++)
        {
            int novaLinha = casaAtual.Linha + direcaoLinha[i];
            int novaColuna = casaAtual.Coluna + direcaoColuna[i];

            // Verifica se a posição está dentro do tabuleiro
            if (novaLinha >= 0 && novaLinha < 8 && novaColuna >= 0 && novaColuna < 8)
            {
                Casa casaDestino = tabuleiro.Casas.First(c => c.Linha == novaLinha && c.Coluna == novaColuna);

                // Verifica se a casa destino está vazia ou tem uma peça adversária
                if (casaDestino.Peca == null || casaDestino.Peca.EBranca != this.EBranca)
                {
                    // Verifica se o movimento não colocaria o rei em xeque
                    if (!tabuleiro.VerificaPerigo(casaDestino, this.EBranca))
                    {
                        movimentos.Add(new Movimento(this, casaAtual, casaDestino, casaDestino.Peca));
                    }
                }
            }
        }

        // Verifica possibilidade de roque
        // Implementar lógica do roque pequeno
        if (VerificaRoque(tabuleiro, true))
        {
            // Adiciona movimento de roque pequeno
            Casa casaDestinoRei = tabuleiro.Casas.First(c => c.Linha == casaAtual.Linha && c.Coluna == casaAtual.Coluna + 2);
            movimentos.Add(new Movimento(this, casaAtual, casaDestinoRei, null, true));
        }

        // Implementar lógica do roque grande
        if (VerificaRoque(tabuleiro, false))
        {
            // Adiciona movimento de roque grande
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
        // Se o rei já foi movido, não pode fazer roque
        if (this.FoiMovimentada)
            return false;

        // Se o rei está em xeque, não pode fazer roque
        if (this.EmCheque)
            return false;

        var casaRei = tabuleiro.ObtemCasaPeca(this);
        if (casaRei == null)
            return false;

        // Verificar se a torre está na posição correta e não foi movida
        int colunaTorre = roquePequeno ? 7 : 0; // Coluna 7 para roque pequeno, coluna 0 para roque grande
        var casaTorre = tabuleiro.Casas.FirstOrDefault(c => c.Linha == casaRei.Linha && c.Coluna == colunaTorre);

        if (casaTorre?.Peca is not ITorre torre || torre.EBranca != this.EBranca || torre.FoiMovimentada)
            return false;

        // Verifica se o caminho entre o rei e a torre está livre
        int direcao = roquePequeno ? 1 : -1;
        int distancia = roquePequeno ? 2 : 3; // Distância a verificar (2 para roque pequeno, 3 para roque grande)

        for (int i = 1; i <= distancia; i++)
        {
            int colunaVerificar = casaRei.Coluna + (i * direcao);
            var casaVerificar = tabuleiro.Casas.First(c => c.Linha == casaRei.Linha && c.Coluna == colunaVerificar);

            // Verifica se há peças no caminho (exceto a torre no final)
            if (i < distancia || (!roquePequeno && i == distancia))
            {
                if (casaVerificar.Peca != null)
                    return false;
            }

            // Verifica se as casas por onde o rei passa não estão sob ataque
            if (i <= 2) // O rei só passa por 2 casas no máximo (sua posição original + 2)
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

        // Determina as posições da torre
        int colunaTorreOrigem = roquePequeno ? 7 : 0;
        int colunaTorreDestino = roquePequeno ? 5 : 3;

        var casaTorreOrigem = tabuleiro.Casas.First(c => c.Linha == casaRei.Linha && c.Coluna == colunaTorreOrigem);
        var casaTorreDestino = tabuleiro.Casas.First(c => c.Linha == casaRei.Linha && c.Coluna == colunaTorreDestino);

        if (casaTorreOrigem.Peca is not ITorre torre)
            throw new InvalidOperationException("Torre não encontrada para executar o roque");

        // Determina as posições do rei
        int colunaReiDestino = roquePequeno ? casaRei.Coluna + 2 : casaRei.Coluna - 2;
        var casaReiDestino = tabuleiro.Casas.First(c => c.Linha == casaRei.Linha && c.Coluna == colunaReiDestino);

        // Cria o movimento de roque
        var movimentoRoque = new Movimento(this, casaRei, casaReiDestino, null, true);

        // Move o rei
        casaRei.Peca = null;
        casaReiDestino.Peca = this;
        this.FoiMovimentada = true;

        // Move a torre
        casaTorreOrigem.Peca = null;
        casaTorreDestino.Peca = torre;
        torre.FoiMovimentada = true;

        return movimentoRoque;
    }
} 