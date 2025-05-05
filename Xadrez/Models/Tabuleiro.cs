using System;
using System.Collections.Generic;
using System.Linq;
using Xadrez.Models.Pecas;

namespace Xadrez.Models
{
    /// <summary>
    /// Representa o tabuleiro de xadrez e gerencia casas, peças, movimentos e capturas.
    /// </summary>
    public class Tabuleiro : ITabuleiro
    {
        /// <summary>
        /// Representa as 64 casas do tabuleiro (8x8).
        /// </summary>
        public List<Casa> Casas { get; } = new List<Casa>();

        /// <summary>
        /// Lista de todas as peças em jogo.
        /// </summary>
        public List<IPeca> Pecas { get; set; } = new List<IPeca>();

        /// <summary>
        /// Lista de peças capturadas durante a partida.
        /// </summary>
        public List<IPeca> PecasCapturadas { get; set; } = new List<IPeca>();

        /// <summary>
        /// Peças brancas ainda em jogo.
        /// </summary>
        public List<IPeca> PecasBrancas => Pecas.Where(p => p.EBranca).ToList();

        /// <summary>
        /// Peças pretas ainda em jogo.
        /// </summary>
        public List<IPeca> PecasPretas => Pecas.Where(p => !p.EBranca).ToList();

        /// <summary>
        /// Peças brancas capturadas.
        /// </summary>
        public List<IPeca> PecasBrancasCapturadas => PecasCapturadas.Where(p => p.EBranca).ToList();

        /// <summary>
        /// Peças pretas capturadas.
        /// </summary>
        public List<IPeca> PecasPretasCapturadas => PecasCapturadas.Where(p => !p.EBranca).ToList();

        /// <summary>
        /// Construtor que inicializa o tabuleiro com 64 casas.
        /// </summary>
        public Tabuleiro()
        {
            for (int linha = 0; linha < 8; linha++)
                for (int coluna = 0; coluna < 8; coluna++)
                    Casas.Add(new Casa(linha, coluna));
        }

        /// <summary>
        /// Distribui todas as peças nas posições iniciais padrão de uma partida de xadrez.
        /// </summary>
        public void DistribuiPecas()
        {
            // Limpa estado anterior
            Pecas.Clear();
            PecasCapturadas.Clear();
            foreach (var casa in Casas)
                casa.Peca = null;

            // Peças brancas
            int linha = 0;
            AdicionaPeca(new Torre(true), linha, 0);
            AdicionaPeca(new Cavalo(true), linha, 1);
            AdicionaPeca(new Bispo(true), linha, 2);
            AdicionaPeca(new Rainha(true), linha, 3);
            AdicionaPeca(new Rei(true), linha, 4);
            AdicionaPeca(new Bispo(true), linha, 5);
            AdicionaPeca(new Cavalo(true), linha, 6);
            AdicionaPeca(new Torre(true), linha, 7);

            linha = 1;
            for (int coluna = 0; coluna < 8; coluna++)
                AdicionaPeca(new Peao(true), linha, coluna);

            // Peças pretas
            linha = 7;
            AdicionaPeca(new Torre(false), linha, 0);
            AdicionaPeca(new Cavalo(false), linha, 1);
            AdicionaPeca(new Bispo(false), linha, 2);
            AdicionaPeca(new Rainha(false), linha, 3);
            AdicionaPeca(new Rei(false), linha, 4);
            AdicionaPeca(new Bispo(false), linha, 5);
            AdicionaPeca(new Cavalo(false), linha, 6);
            AdicionaPeca(new Torre(false), linha, 7);

            linha = 6;
            for (int coluna = 0; coluna < 8; coluna++)
                AdicionaPeca(new Peao(false), linha, coluna);
        }

        /// <summary>
        /// Valida se um movimento é permitido para o jogador, considerando regras de movimento e xeque.
        /// </summary>
        /// <param name="jogador">Jogador que realiza o movimento.</param>
        /// <param name="movimento">Movimento a ser validado.</param>
        /// <returns>Verdadeiro se o movimento for válido; caso contrário, falso.</returns>
        public bool ValidaMovimento(Jogador jogador, Movimento movimento)
        {
            // Verifica se a peça pertence ao jogador
            if (movimento.Peca.EBranca != jogador.EBranco)
                return false;

            // Obtém todos os movimentos possíveis para a peça
            var possiveis = movimento.Peca.MovimentosPossiveis(this);

            // Verifica se o movimento está na lista de movimentos possíveis
            if (!possiveis.Any(m => m.CasaOrigem == movimento.CasaOrigem && m.CasaDestino == movimento.CasaDestino))
                return false;

            // Simula o movimento para verificar se deixa o rei em xeque
            ExecutaMovimento(movimento);
            bool emXeque = VerificaXeque(jogador.EBranco);
            ReverteMovimento(movimento);

            // O movimento é válido se não deixar o rei em xeque
            return !emXeque;
        }

        /// <summary>
        /// Executa o movimento especificado, atualizando posições e capturando peças.
        /// </summary>
        /// <param name="movimento">Movimento a ser executado.</param>
        public void ExecutaMovimento(Movimento movimento)
        {
            // Processa captura de peça, se houver
            if (movimento.PecaCapturada != null)
            {
                Pecas.Remove(movimento.PecaCapturada);
                PecasCapturadas.Add(movimento.PecaCapturada);
            }

            // Move a peça para a casa de destino
            movimento.CasaOrigem.Peca = null;
            movimento.CasaDestino.Peca = movimento.Peca;

            // Marca a peça como movimentada (importante para roque e peões)
            movimento.Peca.FoiMovimentada = true;
        }

        /// <summary>
        /// Reverte um movimento previamente executado, restaurando peças e capturas.
        /// </summary>
        /// <param name="movimento">Movimento a ser revertido.</param>
        public void ReverteMovimento(Movimento movimento)
        {
            // Retorna a peça para a casa de origem
            movimento.CasaOrigem.Peca = movimento.Peca;

            // Restaura a peça capturada, se houver
            movimento.CasaDestino.Peca = movimento.PecaCapturada;

            // Se houve captura, restaura a peça capturada
            if (movimento.PecaCapturada != null)
            {
                PecasCapturadas.Remove(movimento.PecaCapturada);
                Pecas.Add(movimento.PecaCapturada);
            }

            // Desmarca o status de movimentação da peça
            movimento.Peca.FoiMovimentada = false;
        }

        /// <summary>
        /// Retorna a casa onde a peça especificada está posicionada.
        /// </summary>
        /// <param name="peca">Peça a localizar.</param>
        /// <returns>Instância de <see cref="Casa"/> ou nulo se não encontrada.</returns>
        public Casa? ObtemCasaPeca(IPeca peca)
        {
            return Casas.FirstOrDefault(c => c.Peca == peca);
        }

        /// <summary>
        /// Adiciona uma peça em uma posição específica do tabuleiro.
        /// </summary>
        /// <param name="peca">Peça a ser posicionada.</param>
        /// <param name="linha">Índice da linha (0-7).</param>
        /// <param name="coluna">Índice da coluna (0-7).</param>
        private void AdicionaPeca(IPeca peca, int linha, int coluna)
        {
            var casa = Casas.FirstOrDefault(c => c.Linha == linha && c.Coluna == coluna);
            if (casa != null)
            {
                casa.Peca = peca;
                Pecas.Add(peca);
            }
        }

        /// <summary>
        /// Verifica se o rei do jogador está em xeque.
        /// </summary>
        /// <param name="eBranca">Indica se o jogador é branco (true) ou preto (false).</param>
        /// <returns>Verdadeiro se o rei está em xeque; caso contrário, falso.</returns>
        public bool VerificaXeque(bool eBranca)
        {
            // Encontra o rei do jogador
            var rei = Pecas.FirstOrDefault(p => p is IRei && p.EBranca == eBranca);
            if (rei == null) return false;

            var casaRei = ObtemCasaPeca(rei);
            if (casaRei == null) return false;

            // Verifica se alguma peça adversária pode atacar o rei
            var pecasAdversarias = Pecas.Where(p => p.EBranca != eBranca).ToList();

            foreach (var peca in pecasAdversarias)
            {
                var movimentosPossiveis = peca.MovimentosPossiveis(this);
                if (movimentosPossiveis.Any(m => m.CasaDestino == casaRei))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Verifica se o rei do jogador está em xeque-mate.
        /// </summary>
        /// <param name="eBranca">Indica se o jogador é branco (true) ou preto (false).</param>
        /// <returns>Verdadeiro se o rei está em xeque-mate; caso contrário, falso.</returns>
        public bool VerificaXequeMate(bool eBranca)
        {
            // Primeiro verifica se está em xeque
            if (!VerificaXeque(eBranca)) return false;

            // Obtém todas as peças do jogador
            var pecasJogador = Pecas.Where(p => p.EBranca == eBranca).ToList();

            // Verifica se existe algum movimento que tire o jogador do xeque
            foreach (var peca in pecasJogador)
            {
                var casaOrigem = ObtemCasaPeca(peca);
                if (casaOrigem == null) continue;

                var movimentos = peca.MovimentosPossiveis(this);
                foreach (var movimento in movimentos)
                {
                    // Simula o movimento
                    ExecutaMovimento(movimento);
                    bool aindaEmXeque = VerificaXeque(eBranca);
                    ReverteMovimento(movimento);

                    // Se encontrou um movimento que tira do xeque, não é mate
                    if (!aindaEmXeque)
                    {
                        return false;
                    }
                }
            }

            // Se nenhum movimento tirar do xeque, é mate
            return true;
        }

        /// <summary>
        /// Verifica se uma casa específica está sob ataque por peças adversárias.
        /// </summary>
        /// <param name="casa">Casa a ser verificada.</param>
        /// <param name="eBranca">Indica se o jogador é branco (true) ou preto (false).</param>
        /// <returns>Verdadeiro se a casa está sob ataque; caso contrário, falso.</returns>
        public bool VerificaPerigo(Casa casa, bool eBranca)
        {
            // Verifica se alguma peça adversária pode atacar a casa
            var pecasAdversarias = Pecas.Where(p => p.EBranca != eBranca).ToList();

            foreach (var peca in pecasAdversarias)
            {
                var movimentosPossiveis = peca.MovimentosPossiveis(this);
                if (movimentosPossiveis.Any(m => m.CasaDestino == casa))
                {
                    return true;
                }
            }

            return false;
        }
    }
}