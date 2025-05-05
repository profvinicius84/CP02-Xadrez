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
        // Limpa o tabuleiro e a lista de peças
        Pecas.Clear();
        PecasCapturadas.Clear();
        foreach (var casa in Casas)
            casa.Peca = null;

        // Cria e posiciona as peças: brancas e pretas
        // Em um jogo de xadrez completo, aqui seriam criadas todas as peças
        // Para exemplo, vamos criar apenas algumas peças incluindo o Rei

        // Criação dos Reis
        var reiBranco = new Rei(true);
        var reiPreto = new Rei(false);
        
        // Adiciona os reis à lista de peças
        Pecas.Add(reiBranco);
        Pecas.Add(reiPreto);
        
        // Posiciona os reis no tabuleiro
        // Rei branco na posição inicial (e1)
        Casas.First(c => c.Linha == 0 && c.Coluna == 4).Peca = reiBranco;
        
        // Rei preto na posição inicial (e8)
        Casas.First(c => c.Linha == 7 && c.Coluna == 4).Peca = reiPreto;
        
        // Aqui você poderia adicionar outras peças conforme necessário
        // Isso dependerá da implementação completa do jogo
    }

    /// <summary>
    /// Valida se o movimento é válido.
    /// </summary>
    /// <param name="jogador">Jogador que está realizando o movimento.</param>
    /// <param name="movimento">Movimento a ser validado.</param>
    /// <returns>Retorna verdadeiro se o movimento é válido, caso contrário, retorna falso.</returns>
    public bool ValidaMovimento(Jogador jogador, Movimento movimento)
    {
        // Verifica se o movimento é nulo
        if (movimento == null)
            return false;

        // Verifica se a peça pertence ao jogador
        if (movimento.Peca.EBranca != jogador.EBranca)
            return false;
        
        // Verifica se a casa de origem contém a peça do movimento
        var casaOrigem = movimento.CasaOrigem;
        if (casaOrigem.Peca != movimento.Peca)
            return false;
        
        // Se for um movimento de roque, verifica se o roque é válido
        if (movimento.ERoque && movimento.Peca is IRei rei)
        {
            // Verifica se é roque pequeno ou grande
            bool roquePequeno = movimento.CasaDestino.Coluna > movimento.CasaOrigem.Coluna;
            return rei.VerificaRoque(this, roquePequeno);
        }
        
        // Obtém os movimentos possíveis para a peça
        var movimentosPossiveis = movimento.Peca.MovimentosPossiveis(this);
        
        // Verifica se o movimento está na lista de movimentos possíveis
        foreach (var movimentoPossivel in movimentosPossiveis)
        {
            if (movimentoPossivel.CasaDestino == movimento.CasaDestino)
            {
                // Executa o movimento para verificar se coloca o próprio rei em xeque
                ExecutaMovimento(movimento);
                bool colocaReiEmXeque = VerificaXeque(jogador.EBranca);
                ReverteMovimento(movimento);
                
                // Se o movimento colocar o próprio rei em xeque, é inválido
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
        
        // Verifica se é um movimento de roque
        if (movimento.ERoque && peca is IRei rei)
        {
            // Executa o roque
            bool roquePequeno = casaDestino.Coluna > casaOrigem.Coluna;
            rei.ExecutaRoque(this, roquePequeno);
            return;
        }
        
        // Remove a peça da casa de origem
        casaOrigem.Peca = null;
        
        // Se houver uma peça na casa de destino, adiciona à lista de capturadas
        if (casaDestino.Peca != null)
        {
            PecasCapturadas.Add(casaDestino.Peca);
            Pecas.Remove(casaDestino.Peca);
        }
        
        // Coloca a peça na casa de destino
        casaDestino.Peca = peca;
        
        // Marca a peça como movimentada
        peca.FoiMovimentada = true;
        
        // Verifica se o rei adversário está em xeque após o movimento
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
        
        // Verifica se é um movimento de roque
        if (movimento.ERoque && peca is IRei)
        {
            // Reverte o roque
            bool roquePequeno = casaDestino.Coluna > casaOrigem.Coluna;
            
            // Reverte o movimento do rei
            casaDestino.Peca = null;
            casaOrigem.Peca = peca;
            peca.FoiMovimentada = false;
            
            // Reverte o movimento da torre
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
        
        // Remove a peça da casa de destino
        casaDestino.Peca = null;
        
        // Se houver uma peça capturada, remove da lista de capturadas e adiciona de volta
        if (movimento.PecaCapturada != null)
        {
            casaDestino.Peca = movimento.PecaCapturada;
            PecasCapturadas.Remove(movimento.PecaCapturada);
            Pecas.Add(movimento.PecaCapturada);
        }
        
        // Coloca a peça de volta na casa de origem
        casaOrigem.Peca = peca;
        
        // Desmarca a peça como movimentada se for o primeiro movimento
        // (Não sabemos se foi o primeiro movimento, então seria necessário manter um histórico completo)
        // Para simplificar, deixamos como está
        
        // Reseta o estado de xeque para todos os reis
        foreach (var pecaTabuleiro in Pecas)
        {
            if (pecaTabuleiro is IRei reiTabuleiro)
                reiTabuleiro.EmCheque = VerificaXeque(reiTabuleiro.EBranca);
        }
    }

    public Casa? ObtemCasaPeca(IPeca peca)
    {
        return Casas.FirstOrDefault(c => c.Peca == peca);
    }

    /// <summary>
    /// Verifica se um jogador está em xeque. O xeque ocorre quando o rei do jogador está ameaçado por uma peça adversária.
    /// </summary>
    /// <param name="eBranca">Indica se o jogador é branco ou preto. Se for verdadeiro, o jogador é branco. Se for falso, o jogador é preto.</param>
    /// <returns>Retorna verdadeiro se o jogador está em xeque, caso contrário, retorna falso.</returns>
    public bool VerificaXeque(bool eBranca)
    {
        // Encontra o rei do jogador
        var rei = Pecas.FirstOrDefault(p => p is IRei && p.EBranca == eBranca) as IRei;
        if (rei == null)
            return false; // Se não houver rei, não está em xeque (caso especial)

        // Obtem a casa do rei
        var casaRei = ObtemCasaPeca(rei as IPeca);
        if (casaRei == null)
            return false;

        // Verifica se alguma peça adversária pode capturar o rei
        return VerificaPerigo(casaRei, eBranca);
    }

    /// <summary>
    /// Verifica se um jogador está em xeque-mate. O xeque-mate ocorre quando o rei do jogador está ameaçado e não há movimentos possíveis para escapar do xeque.
    /// </summary>
    /// <param name="eBranca">Indica se o jogador é branco ou preto. Se for verdadeiro, o jogador é branco. Se for falso, o jogador é preto.</param>
    /// <returns>Retorna verdadeiro se o jogador está em xeque-mate, caso contrário, retorna falso.</returns>
    public bool VerificaXequeMate(bool eBranca)
    {
        // Primeiro verifica se o rei está em xeque
        if (!VerificaXeque(eBranca))
            return false;

        // Encontra o rei do jogador
        var rei = Pecas.FirstOrDefault(p => p is IRei && p.EBranca == eBranca);
        if (rei == null)
            return false;

        // Verifica se há algum movimento possível para o rei
        var movimentosPossiveisRei = rei.MovimentosPossiveis(this);
        if (movimentosPossiveisRei.Count > 0)
            return false; // Se o rei puder se mover, não é xeque-mate

        // Verifica se alguma outra peça pode bloquear o xeque ou capturar a peça que está dando xeque
        var pecasDoJogador = Pecas.Where(p => p.EBranca == eBranca && !(p is IRei)).ToList();
        foreach (var peca in pecasDoJogador)
        {
            var movimentosPossiveis = peca.MovimentosPossiveis(this);
            if (movimentosPossiveis.Count > 0)
            {
                foreach (var movimento in movimentosPossiveis)
                {
                    // Simula o movimento para verificar se o rei continua em xeque
                    ExecutaMovimento(movimento);
                    bool aindaEmXeque = VerificaXeque(eBranca);
                    ReverteMovimento(movimento);

                    if (!aindaEmXeque)
                        return false; // Se este movimento tirar o rei do xeque, não é xeque-mate
                }
            }
        }

        // Se chegou até aqui, é xeque-mate
        return true;
    }

    /// <summary>
    /// Verifica se uma casa está sob ataque. Uma casa está sob ataque se uma peça adversária pode se mover para essa casa.
    /// </summary>
    /// <param name="casa">Casa a ser verificada.</param>
    /// <param name="eBranca">Indica se o jogador é branco ou preto. Se for verdadeiro, o jogador é branco. Se for falso, o jogador é preto.</param>
    /// <returns>Retorna verdadeiro se a casa está sob ataque, caso contrário, retorna falso.</returns>
    public bool VerificaPerigo(Casa casa, bool eBranca)
    {
        // Obtém todas as peças adversárias
        List<IPeca> pecasAdversarias = eBranca ? PecasPretas : PecasBrancas;

        // Verifica se alguma peça adversária pode se mover para a casa em questão
        foreach (var pecaAdversaria in pecasAdversarias)
        {
            // Verifica de acordo com o tipo de peça para evitar recursão infinita
            if (VerificaAtaquePeca(pecaAdversaria, casa))
                return true;
        }

        return false;
    }

    /// <summary>
    /// Verifica se uma peça pode atacar uma determinada casa, baseado no tipo de peça e sua posição.
    /// </summary>
    /// <param name="peca">A peça a ser verificada.</param>
    /// <param name="casaAlvo">A casa alvo que pode estar sob ataque.</param>
    /// <returns>Retorna verdadeiro se a peça pode atacar a casa, caso contrário, retorna falso.</returns>
    private bool VerificaAtaquePeca(IPeca peca, Casa casaAlvo)
    {
        var casaPeca = ObtemCasaPeca(peca);
        if (casaPeca == null)
            return false;

        // Verificação para evitar loop infinito quando chamado de dentro do MovimentosPossiveis do Rei
        // Se estamos verificando a casa de uma peça adversária, não há ataque
        if (casaPeca == casaAlvo)
            return false;

        // Lógica específica para cada tipo de peça
        if (peca is Hades)
        {
            // Hades pode atacar qualquer casa
            return true;
        }
        else if (peca is IRei)
        {
            // Rei pode atacar casas adjacentes (1 casa em qualquer direção)
            int difLinha = Math.Abs(casaPeca.Linha - casaAlvo.Linha);
            int difColuna = Math.Abs(casaPeca.Coluna - casaAlvo.Coluna);
            return difLinha <= 1 && difColuna <= 1 && (difLinha > 0 || difColuna > 0);
        }
        else if (peca is ITorre)
        {
            // Torre pode atacar em linhas retas (horizontal e vertical)
            if (casaPeca.Linha == casaAlvo.Linha || casaPeca.Coluna == casaAlvo.Coluna)
            {
                // Verifica se há peças no caminho
                return CaminhoLivre(casaPeca, casaAlvo);
            }
            return false;
        }
        else if (peca is IBispo)
        {
            // Bispo pode atacar na diagonal
            int difLinha = Math.Abs(casaPeca.Linha - casaAlvo.Linha);
            int difColuna = Math.Abs(casaPeca.Coluna - casaAlvo.Coluna);
            if (difLinha == difColuna)
            {
                // Verifica se há peças no caminho
                return CaminhoLivre(casaPeca, casaAlvo);
            }
            return false;
        }
        else if (peca is IRainha)
        {
            // Rainha pode atacar em qualquer direção (horizontal, vertical e diagonal)
            int difLinha = Math.Abs(casaPeca.Linha - casaAlvo.Linha);
            int difColuna = Math.Abs(casaPeca.Coluna - casaAlvo.Coluna);
            if (casaPeca.Linha == casaAlvo.Linha || casaPeca.Coluna == casaAlvo.Coluna || difLinha == difColuna)
            {
                // Verifica se há peças no caminho
                return CaminhoLivre(casaPeca, casaAlvo);
            }
            return false;
        }
        else if (peca is ICavalo)
        {
            // Cavalo pode atacar em L (2 casas em uma direção e 1 casa na outra)
            int difLinha = Math.Abs(casaPeca.Linha - casaAlvo.Linha);
            int difColuna = Math.Abs(casaPeca.Coluna - casaAlvo.Coluna);
            return (difLinha == 2 && difColuna == 1) || (difLinha == 1 && difColuna == 2);
        }
        else if (peca is IPeao)
        {
            // Peão pode atacar na diagonal à frente
            int direcao = peca.EBranca ? 1 : -1;
            return (casaAlvo.Linha == casaPeca.Linha + direcao) && 
                   (casaAlvo.Coluna == casaPeca.Coluna - 1 || casaAlvo.Coluna == casaPeca.Coluna + 1);
        }

        // Se não é nenhum dos tipos específicos, verifica através dos movimentos possíveis
        var movimentosPossiveis = peca.MovimentosPossiveis(this);
        return movimentosPossiveis.Any(m => m.CasaDestino == casaAlvo);
    }

    /// <summary>
    /// Verifica se o caminho entre duas casas está livre de peças.
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