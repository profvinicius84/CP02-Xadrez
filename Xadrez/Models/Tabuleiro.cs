using Xadrez.Models.Pecas;

namespace Xadrez.Models;

/// <summary>
/// Representa o tabuleiro de xadrez.
/// </summary>
public class Tabuleiro : ITabuleiro
{
    /// <summary>
    /// Representa o tamanho do tabuleiro de xadrez.
    /// </summary>
    public List<Casa> Casas { get; } = new();

    /// <summary>
    /// Pecas das partida
    /// </summary>
    public List<IPeca> Pecas { get; set; } = new();

    /// <summary>
    /// Pecas brancas da partida
    /// </summary>
    public List<IPeca> PecasBrancas => Pecas.Where(p => p.EBranca).ToList();

    /// <summary>
    /// Pecas pretas da partida
    /// </summary>
    public List<IPeca> PecasPretas => Pecas.Where(p => !p.EBranca).ToList();

    /// <summary>
    /// Representa a lista de peças capturadas durante a partida.
    /// </summary>
    public List<IPeca> PecasCapturadas { get; set; } = new();

    /// <summary>
    /// Representa a lista de peças brancas capturadas durante a partida.
    /// </summary>
    public List<IPeca> PecasBrancasCapturadas => PecasCapturadas.Where(p => p.EBranca).ToList();

    /// <summary>
    /// Representa a lista de peças pretas capturadas durante a partida.
    /// </summary>
    public List<IPeca> PecasPretasCapturadas => PecasCapturadas.Where(p => !p.EBranca).ToList();

    /// <summary>
    /// Construtor que inicializa o tabuleiro de xadrez com 64 casas.
    /// </summary>
    public Tabuleiro()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                var casa = new Casa(i, j);
                Casas.Add(casa);
            }
        }
    }

    /// <summary>
    /// Cria e distribui as peças no tabuleiro.
    /// </summary>
    public void DistribuiPecas()
    {
        Pecas.Clear();
        PecasCapturadas.Clear();
        foreach (var casa in Casas)
            casa.Peca = null;

        var reiBranco = new Rei(true);
        var reiPreto = new Rei(false);
        
        Pecas.Add(reiBranco);
        Pecas.Add(reiPreto);
        
        Casas.First(c => c.Linha == 0 && c.Coluna == 4).Peca = reiBranco;
        Casas.First(c => c.Linha == 7 && c.Coluna == 4).Peca = reiPreto;
    }

    /// <summary>
    /// Valida se o movimento é válido.
    /// </summary>
    /// <param name="jogador">Jogador que está realizando o movimento.</param>
    /// <param name="movimento">Movimento a ser validado.</param>
    /// <returns>Retorna verdadeiro se o movimento é válido, caso contrário, retorna falso.</returns>
    public bool ValidaMovimento(Jogador jogador, Movimento movimento)
    {
        if (movimento == null)
            return false;

        if (movimento.Peca.EBranca != jogador.EBranca)
            return false;
        
        var casaOrigem = movimento.CasaOrigem;
        if (casaOrigem.Peca != movimento.Peca)
            return false;
        
        if (movimento.ERoque && movimento.Peca is IRei rei)
        {
            bool roquePequeno = movimento.CasaDestino.Coluna > movimento.CasaOrigem.Coluna;
            return rei.VerificaRoque(this, roquePequeno);
        }
        
        var movimentosPossiveis = movimento.Peca.MovimentosPossiveis(this);
        
        foreach (var movimentoPossivel in movimentosPossiveis)
        {
            if (movimentoPossivel.CasaDestino == movimento.CasaDestino)
            {
                ExecutaMovimento(movimento);
                bool colocaReiEmXeque = VerificaXeque(jogador.EBranca);
                ReverteMovimento(movimento);
                
                if (colocaReiEmXeque)
                    return false;
                
                return true;
            }
        }
        
        return false;
    }

    /// <summary>
    /// Executa o movimento no tabuleiro. O movimento é composto por uma peça e uma casa de destino. O movimento pode ser um movimento normal ou um roque (pequeno ou grande).
    /// </summary>
    /// <param name="movimento">Movimento a ser executado.</param>
    public void ExecutaMovimento(Movimento movimento)
    {
        var casaOrigem = movimento.CasaOrigem;
        var casaDestino = movimento.CasaDestino;
        var peca = movimento.Peca;
        
        if (movimento.ERoque && peca is IRei rei)
        {
            bool roquePequeno = casaDestino.Coluna > casaOrigem.Coluna;
            rei.ExecutaRoque(this, roquePequeno);
            return;
        }
        
        casaOrigem.Peca = null;
        
        if (casaDestino.Peca != null)
        {
            PecasCapturadas.Add(casaDestino.Peca);
            Pecas.Remove(casaDestino.Peca);
        }
        
        casaDestino.Peca = peca;
        peca.FoiMovimentada = true;
        
        bool corAdversaria = !peca.EBranca;
        if (VerificaXeque(corAdversaria))
        {
            var reiAdversario = Pecas.FirstOrDefault(p => p is IRei && p.EBranca == corAdversaria) as IRei;
            if (reiAdversario != null)
                reiAdversario.EmCheque = true;
        }
    }

    /// <summary>
    /// Reverte o movimento no tabuleiro. O movimento é composto por uma peça e uma casa de destino. O movimento pode ser um movimento normal ou um roque (pequeno ou grande).
    /// </summary>
    /// <param name="movimento">Movimento a ser revertido.</param>
    public void ReverteMovimento(Movimento movimento)
    {
        var casaOrigem = movimento.CasaOrigem;
        var casaDestino = movimento.CasaDestino;
        var peca = movimento.Peca;
        
        if (movimento.ERoque && peca is IRei)
        {
            bool roquePequeno = casaDestino.Coluna > casaOrigem.Coluna;
            
            casaDestino.Peca = null;
            casaOrigem.Peca = peca;
            peca.FoiMovimentada = false;
            
            int colunaTorreOrigem = roquePequeno ? 7 : 0;
            int colunaTorreDestino = roquePequeno ? 5 : 3;
            
            var casaTorreOrigem = Casas.First(c => c.Linha == casaOrigem.Linha && c.Coluna == colunaTorreOrigem);
            var casaTorreDestino = Casas.First(c => c.Linha == casaOrigem.Linha && c.Coluna == colunaTorreDestino);
            
            var torre = casaTorreDestino.Peca;
            casaTorreDestino.Peca = null;
            casaTorreOrigem.Peca = torre;
            
            if (torre != null)
                torre.FoiMovimentada = false;
            
            return;
        }
        
        casaDestino.Peca = null;
        
        if (movimento.PecaCapturada != null)
        {
            casaDestino.Peca = movimento.PecaCapturada;
            PecasCapturadas.Remove(movimento.PecaCapturada);
            Pecas.Add(movimento.PecaCapturada);
        }
        
        casaOrigem.Peca = peca;
        
        foreach (var pecaTabuleiro in Pecas)
        {
            if (pecaTabuleiro is IRei reiTabuleiro)
                reiTabuleiro.EmCheque = VerificaXeque(reiTabuleiro.EBranca);
        }
    }

    /// <summary>
    /// Obtém a casa onde está uma peça.
    /// </summary>
    /// <param name="peca">Peça a ser localizada.</param>
    /// <returns>Retorna a casa onde está a peça, ou null se a peça não estiver no tabuleiro.</returns>
    public Casa? ObtemCasaPeca(IPeca peca)
    {
        return Casas.FirstOrDefault(c => c.Peca == peca);
    }

    /// <summary>
    /// Verifica se um jogador está em xeque.
    /// </summary>
    /// <param name="eBranca">Indica se o jogador é branco ou preto.</param>
    /// <returns>Retorna verdadeiro se o jogador está em xeque, caso contrário, retorna falso.</returns>
    public bool VerificaXeque(bool eBranca)
    {
        var rei = Pecas.FirstOrDefault(p => p is IRei && p.EBranca == eBranca) as IRei;
        if (rei == null)
            return false;

        var casaRei = ObtemCasaPeca(rei as IPeca);
        if (casaRei == null)
            return false;

        return VerificaPerigo(casaRei, eBranca);
    }

    /// <summary>
    /// Verifica se um jogador está em xeque-mate.
    /// </summary>
    /// <param name="eBranca">Indica se o jogador é branco ou preto.</param>
    /// <returns>Retorna verdadeiro se o jogador está em xeque-mate, caso contrário, retorna falso.</returns>
    public bool VerificaXequeMate(bool eBranca)
    {
        if (!VerificaXeque(eBranca))
            return false;

        var rei = Pecas.FirstOrDefault(p => p is IRei && p.EBranca == eBranca);
        if (rei == null)
            return false;

        var movimentosPossiveisRei = rei.MovimentosPossiveis(this);
        if (movimentosPossiveisRei.Count > 0)
            return false;

        var pecasDoJogador = Pecas.Where(p => p.EBranca == eBranca && !(p is IRei)).ToList();
        foreach (var peca in pecasDoJogador)
        {
            var movimentosPossiveis = peca.MovimentosPossiveis(this);
            if (movimentosPossiveis.Count > 0)
            {
                foreach (var movimento in movimentosPossiveis)
                {
                    ExecutaMovimento(movimento);
                    bool aindaEmXeque = VerificaXeque(eBranca);
                    ReverteMovimento(movimento);

                    if (!aindaEmXeque)
                        return false;
                }
            }
        }

        return true;
    }

    /// <summary>
    /// Verifica se uma casa está sob ataque.
    /// </summary>
    /// <param name="casa">Casa a ser verificada.</param>
    /// <param name="eBranca">Indica se o jogador é branco ou preto.</param>
    /// <returns>Retorna verdadeiro se a casa está sob ataque, caso contrário, retorna falso.</returns>
    public bool VerificaPerigo(Casa casa, bool eBranca)
    {
        List<IPeca> pecasAdversarias = eBranca ? PecasPretas : PecasBrancas;

        foreach (var pecaAdversaria in pecasAdversarias)
        {
            if (VerificaAtaquePeca(pecaAdversaria, casa))
                return true;
        }

        return false;
    }

    /// <summary>
    /// Verifica se uma peça pode atacar uma casa.
    /// </summary>
    /// <param name="peca">Peça a ser verificada.</param>
    /// <param name="casaAlvo">Casa alvo do ataque.</param>
    /// <returns>Retorna verdadeiro se a peça pode atacar a casa, caso contrário, retorna falso.</returns>
    private bool VerificaAtaquePeca(IPeca peca, Casa casaAlvo)
    {
        var casaPeca = ObtemCasaPeca(peca);
        if (casaPeca == null)
            return false;

        if (casaPeca == casaAlvo)
            return false;

        if (peca is Hades)
        {
            return true;
        }
        else if (peca is IRei)
        {
            int difLinha = Math.Abs(casaPeca.Linha - casaAlvo.Linha);
            int difColuna = Math.Abs(casaPeca.Coluna - casaAlvo.Coluna);
            return difLinha <= 1 && difColuna <= 1 && (difLinha > 0 || difColuna > 0);
        }
        else if (peca is ITorre)
        {
            if (casaPeca.Linha == casaAlvo.Linha || casaPeca.Coluna == casaAlvo.Coluna)
            {
                return CaminhoLivre(casaPeca, casaAlvo);
            }
            return false;
        }
        else if (peca is IBispo)
        {
            int difLinha = Math.Abs(casaPeca.Linha - casaAlvo.Linha);
            int difColuna = Math.Abs(casaPeca.Coluna - casaAlvo.Coluna);
            if (difLinha == difColuna)
            {
                return CaminhoLivre(casaPeca, casaAlvo);
            }
            return false;
        }
        else if (peca is IRainha)
        {
            int difLinha = Math.Abs(casaPeca.Linha - casaAlvo.Linha);
            int difColuna = Math.Abs(casaPeca.Coluna - casaAlvo.Coluna);
            if (casaPeca.Linha == casaAlvo.Linha || casaPeca.Coluna == casaAlvo.Coluna || difLinha == difColuna)
            {
                return CaminhoLivre(casaPeca, casaAlvo);
            }
            return false;
        }
        else if (peca is ICavalo)
        {
            int difLinha = Math.Abs(casaPeca.Linha - casaAlvo.Linha);
            int difColuna = Math.Abs(casaPeca.Coluna - casaAlvo.Coluna);
            return (difLinha == 2 && difColuna == 1) || (difLinha == 1 && difColuna == 2);
        }
        else if (peca is IPeao)
        {
            int direcao = peca.EBranca ? 1 : -1;
            return (casaAlvo.Linha == casaPeca.Linha + direcao) && 
                   (casaAlvo.Coluna == casaPeca.Coluna - 1 || casaAlvo.Coluna == casaPeca.Coluna + 1);
        }

        var movimentosPossiveis = peca.MovimentosPossiveis(this);
        return movimentosPossiveis.Any(m => m.CasaDestino == casaAlvo);
    }

    /// <summary>
    /// Verifica se o caminho entre duas casas está livre.
    /// </summary>
    /// <param name="origem">Casa de origem.</param>
    /// <param name="destino">Casa de destino.</param>
    /// <returns>Retorna verdadeiro se o caminho está livre, caso contrário, retorna falso.</returns>
    private bool CaminhoLivre(Casa origem, Casa destino)
    {
        int difLinha = destino.Linha - origem.Linha;
        int difColuna = destino.Coluna - origem.Coluna;
        
        int passoLinha = difLinha == 0 ? 0 : difLinha / Math.Abs(difLinha);
        int passoColuna = difColuna == 0 ? 0 : difColuna / Math.Abs(difColuna);
        
        int linhaAtual = origem.Linha + passoLinha;
        int colunaAtual = origem.Coluna + passoColuna;
        
        while (linhaAtual != destino.Linha || colunaAtual != destino.Coluna)
        {
            var casaAtual = Casas.First(c => c.Linha == linhaAtual && c.Coluna == colunaAtual);
            if (casaAtual.Peca != null)
                return false;
            
            linhaAtual += passoLinha;
            colunaAtual += passoColuna;
        }
        
        return true;
    }
}