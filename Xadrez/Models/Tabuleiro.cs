using System.Linq;
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

    //Feito
    public void DistribuiPecas()
    {
        Pecas.Clear();

        foreach (var casa in Casas)
            casa.Peca = null;

        // Helper para achar uma casa
        Casa GetCasa(int linha, int coluna) =>
            Casas.Single(c => c.Linha == linha && c.Coluna == coluna);

        //Peões brancos na linha 1
        for (int col = 0; col < 8; col++)
        {
            var p = new Peao(true);
            var casa = GetCasa(1, col);
            casa.Peca = p;
            Pecas.Add(p);
        }

        //Peças da linha 0 (torre, cavalo, bispo, dama, rei, bispo, cavalo, torre)
        IPeca[] ordemBackRankBranca = {
            new Torre(true),  new Cavalo(true), new Bispo(true),  new Rainha(true),
            new Rei(true),    new Bispo(true),  new Cavalo(true), new Torre(true)
        };

        for (int col = 0; col < 8; col++)
        {
            var p = ordemBackRankBranca[col];
            var casa = GetCasa(0, col);
            casa.Peca = p;
            Pecas.Add(p);
        }

        //Peões pretos na linha 6
        for (int col = 0; col < 8; col++)
        {
            var p = new Peao(false);
            var casa = GetCasa(6, col);
            casa.Peca = p;
            Pecas.Add(p);
        }

        //Peças da linha 7 (igual à branca, mas com cor preta)
        IPeca[] ordemBackRankPreta = {
            new Torre(false),  new Cavalo(false), new Bispo(false),  new Rainha(false),
            new Rei(false),    new Bispo(false),   new Cavalo(false), new Torre(false)
        };
        for (int col = 0; col < 8; col++)
        {
            var p = ordemBackRankPreta[col];
            var casa = GetCasa(7, col);
            casa.Peca = p;
            Pecas.Add(p);
        }
    }
    
    //Feito
    public bool ValidaMovimento(Jogador jogador, Movimento movimento)
    {
        // Verificando se a peça que quer mexer é da mesma cor que o jogador
        if (movimento.Peca.EBranca != jogador.EBranco)
            return false;

        //Verifica se a peça realmente está na casa de origem indicada
        if (movimento.CasaOrigem.Peca != movimento.Peca)
            return false;

        //Verifica se a casa destino já possui uma peça, e se essa peça é da mesma cor do jogador
        if (movimento.CasaDestino.Peca != null && movimento.CasaDestino.Peca.EBranca == jogador.EBranco)
            return false;

        //Verifica se o movimento está entre os movimento válidos da peça
        if (!movimento.Peca.MovimentosPossiveis(this).Any(m => m.CasaDestino == movimento.CasaDestino))
            return false;

        // Guarda as referências às casas e peças envolvidas
        var origem = movimento.CasaOrigem;
        var destino = movimento.CasaDestino;

        var pecaOrigem = origem.Peca;
        var pecaDestino = destino.Peca;

        // Aplica o movimento no tabuleiro (temporariamente)
        origem.Peca = null;
        destino.Peca = pecaOrigem;

        // Verifica se, após o movimento, o rei do jogador está em xeque

        bool reiEmXeque = VerificaXeque(jogador.EBranco);

        // Desfaz o movimento
        origem.Peca = pecaOrigem;
        destino.Peca = pecaDestino;

        if (reiEmXeque)
            return false;

        // Se for um movimento de roque, valida se o roque é permitido

        if (movimento.ERoque && movimento.Peca is Rei rei)
        {
            if (!rei.VerificaRoque(this))
                return false;
        }

        return true;
    }

    //Feito
    public void ExecutaMovimento(Movimento movimento)
    {

        var origem = movimento.CasaOrigem;
        var destino = movimento.CasaDestino;

        var pecaOrigem = origem.Peca;
        var pecaDestino = destino.Peca;

        if(destino.Peca != null)
        {
            if (origem.Peca.EBranca)
            {
                PecasPretas.Remove(destino.Peca);
                PecasPretasCapturadas.Add(destino.Peca);
            }
            else
            {
                PecasBrancas.Remove(destino.Peca);
                PecasBrancasCapturadas.Add(destino.Peca);
            }
        }

        origem.Peca = null;
        destino.Peca = pecaOrigem;
        


        if (movimento.Peca is Rei rei)
        {
            if(!rei.FoiMovimentada && movimento.ERoque)
            {
                rei.ExecutaRoque(this);
            }
            else
            {
                rei.FoiMovimentada = true;
            }
                
        }
    }

    public void ReverteMovimento(Movimento movimento)
    {
        var origem = movimento.CasaOrigem;
        var destino = movimento.CasaDestino;

        origem.Peca = movimento.Peca;
        destino.Peca = movimento.PecaCapturada;
        

        if (movimento.PecaCapturada != null)
        {
            if (origem.Peca.EBranca)
            {
                PecasPretasCapturadas.Remove(movimento.PecaCapturada);
                PecasPretas.Add(movimento.PecaCapturada);
            }
            else
            {
                PecasBrancasCapturadas.Remove(movimento.PecaCapturada);
                PecasBrancas.Add(movimento.PecaCapturada);
            }
        }

        // Se for um roque, execute a reversão do rei e torre
        // Se o movimento for um roque, precisamos desfazer os movimentos do rei e torre
        if (movimento.ERoque)
        {
            if (movimento.Peca is Rei rei)
            {
                // Verifique se foi roque pequeno ou grande, dependendo da posição do movimento
                if (movimento.CasaOrigem.Codigo == "e1" && movimento.CasaDestino.Codigo == "c1") // Exemplo de roque grande (preto)
                {
                    // Restaurar a posição do rei
                    var casaRei = ObtemCasaPeca(rei);
                    var casaReiOriginal = ObtemCasaPeca(rei); // Coloque aqui a casa original do rei
                    casaRei.Peca = null;
                    casaReiOriginal.Peca = rei;

                    // Restaurar a posição da torre
                    var torre = movimento.CasaDestino.Peca as Torre;
                    var casaTorre = ObtemCasaPeca(torre);
                    var casaTorreOriginal = ObtemCasaPeca(torre); // Coloque aqui a casa original da torre
                    casaTorre.Peca = null;
                    casaTorreOriginal.Peca = torre;
                }
                else if (movimento.CasaOrigem.Codigo == "e1" && movimento.CasaDestino.Codigo == "g1") // Exemplo de roque pequeno (branco)
                {
                    // Restaurar a posição do rei
                    var casaRei = ObtemCasaPeca(rei);
                    var casaReiOriginal = ObtemCasaPeca(rei); // Coloque aqui a casa original do rei
                    casaRei.Peca = null;
                    casaReiOriginal.Peca = rei;

                    // Restaurar a posição da torre
                    var torre = movimento.CasaDestino.Peca as Torre;
                    var casaTorre = ObtemCasaPeca(torre);
                    var casaTorreOriginal = ObtemCasaPeca(torre); // Coloque aqui a casa original da torre
                    casaTorre.Peca = null;
                    casaTorreOriginal.Peca = torre;
                }
            }
        }

    }

    public Casa? ObtemCasaPeca(IPeca peca)
    {
        return Casas.FirstOrDefault(c => c.Peca == peca);
    }

    public bool VerificaXeque(bool eBranca)
    {
        // Seleciona as peças do jogador conforme a cor informada
        var pecasDoJogador = eBranca ? PecasBrancas : PecasPretas;

        // Encontra o rei do jogador
        IPeca? rei = pecasDoJogador.FirstOrDefault(p => p is IRei);
        if (rei == null)
        {
            // Não encontrou o rei — talvez o jogo esteja em um estado inválido
            return false;
        }

        // Encontra a casa onde está o rei
        Casa? casaDoRei = ObtemCasaPeca(rei);
        if (casaDoRei == null)
        {
            // O rei não está posicionado em nenhuma casa — também um estado inválido
            return false;
        }

        // Seleciona as peças inimigas
        var pecasInimigas = eBranca ? PecasPretas : PecasBrancas;

        // Verifica se alguma peça inimiga pode capturar o rei
        foreach (var pecaInimiga in pecasInimigas)
        {
            var movimentosInimigos = pecaInimiga.MovimentosPossiveis(this);
            foreach (var movimento in movimentosInimigos)
            {
                if (movimento.CasaDestino == casaDoRei)
                {
                    // O rei está sob ameaça
                    return true;
                }
            }
        }

        // Nenhuma peça inimiga ameaça o rei
        return false;
    }

    public bool VerificaXequeMate(bool eBranca)
    {
        throw new NotImplementedException();
    }

    public bool VerificaPerigo(Casa casa, bool eBranca)
    {
        throw new NotImplementedException();
    }
}