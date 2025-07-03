using GestãoEmpresarial.Interface;
using GestãoEmpresarial.Models;
using GestãoEmpresarial.Providers;
using GestãoEmpresarial.Repositorios;
using GestãoEmpresarial.Validations;
using Google.Protobuf.WellKnownTypes;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using MaterialDesignThemes.Wpf;
using MicroMvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Wrapper.WhatsGW;

namespace GestãoEmpresarial.ViewModels
{
    internal class CadastroOrdemServicoViewModel : CadastroViewModel<OrdemServicoModel, EditarOsModel>
    {
        private readonly RCodigosDAL _codigosDal;
        private readonly RItensOSDAL _itensOsDal;
        private readonly ItemOrdemServicoValidar _itemOsvalidador;
        private readonly ROsDAL _ordemServicoRepositorio;

        private readonly CodigoModel StatusCancelado;

        public ICommand CancelarOsCommand { get; set; }// Novo

        public CadastroOrdemServicoViewModel(int? id, OrdemServicoValidar validar, ROsDAL repositorio, ItemOrdemServicoValidar itemOsvalidador, RCodigosDAL codigosDAL, RItensOSDAL itensOSDAL)
           : base(id, validar, repositorio)
        {
            _itemOsvalidador = itemOsvalidador;
            _codigosDal = codigosDAL;
            _itensOsDal = itensOSDAL;
            _ordemServicoRepositorio = repositorio;  // Armazene o valor do repositório

            AdicionarItemOsCommand = new RelayCommandWithParameterAsync(ExecutarGuardarItemNaListaAsync, CanExecuteAdicionarItem);

            ApagarItemOsCommand = new RelayCommandWithParameterAsync(ExecutarApagarItemGridNaListaAsync, CanExecuteApagarItem);

            CancelarOsCommand = new RelayCommandWithParameterAsync(ExecutarCancelarOsAsync, CanExecuteCancelarOs); // Novo comando
            ProdutoProviderItem = new ProdutoProvider();

            StatusCancelado = _codigosDal.GetStatusCanceladaAsync().Result;

            // Chamada assíncrona ao inicializar
            InitializeAsync(id).ConfigureAwait(false);


            if (ObjectoEditar.Finalizado)
            {
                if (LoginViewModel.Instancia.colaborador.Cargo.ToUpper().Equals("GERENTE") == false)
                    ApenasVisualizar = true;
            }
        }

        public bool PodeCancelar => LoginViewModel.Instancia.colaborador.Cargo.ToUpper() == "GERENTE";

        public ProdutoProvider ProdutoProviderItem { get; set; }

        public ICommand AdicionarItemOsCommand { get; set; }

        public ICommand ApagarItemOsCommand { get; set; }

        public Dictionary<int, string> MarcasList { get; internal set; }

        public Dictionary<int, string> StatusList { get; internal set; }

        public bool PodeEditar { get; internal set; }

        public bool NovaOrdemServico { get; internal set; }

        // NOVO MÉTODO PARA GERAR O PDF EM MEMÓRIA
        //private byte[] GerarPdfDaOs(EditarOsModel os)
        //{
        //    try
        //    {
        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            Document doc = new Document(PageSize.A4, 40, 40, 30, 30);
        //            PdfWriter writer = PdfWriter.GetInstance(doc, ms);
        //            doc.Open();

        //            var bold = new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD);
        //            var normal = new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL);
        //            var titleFont = new Font(Font.FontFamily.HELVETICA, 16, Font.BOLD);

        //            //1.CABEÇALHO DA EMPRESA
        //            var empresaFont = new Font(Font.FontFamily.HELVETICA, 16, Font.BOLD, BaseColor.DARK_GRAY);
        //            var empresaHeader = new Paragraph("FORTE MÁQUINAS", empresaFont)
        //            {
        //                Alignment = Element.ALIGN_CENTER,
        //                SpacingAfter = 5f
        //            };
        //            doc.Add(empresaHeader);


        //            var subHeaderFont = new Font(Font.FontFamily.HELVETICA, 12, Font.NORMAL, BaseColor.GRAY);
        //            var subHeader = new Paragraph("ORDEM DE SERVIÇO", subHeaderFont)
        //            {
        //                Alignment = Element.ALIGN_CENTER,
        //                SpacingAfter = 20f
        //            };
        //            doc.Add(subHeader);

        //            // Linha divisória
        //            doc.Add(new Chunk(new LineSeparator(1f, 100f, BaseColor.LIGHT_GRAY, Element.ALIGN_CENTER, -1)));
        //            doc.Add(Chunk.NEWLINE);

        //            // Status, Marca e Número da OS
        //            string statusDescricao = _codigosDal.GetByIdAsync(os.Status).Result?.Descricao ?? "N/D";
        //            string marcaDescricao = _codigosDal.GetByIdAsync(os.Marca).Result?.Descricao ?? "N/D";

        //            Paragraph infoHeader = new Paragraph
        //            {
        //                Alignment = Element.ALIGN_CENTER,
        //                SpacingAfter = 10f
        //            };
        //            doc.Add(new Paragraph($"Status: {statusDescricao.ToUpper()} ", bold));
        //            doc.Add(new Paragraph($"OS: Nº {os.IdOs}", bold));

        //            //infoHeader.Add(new Chunk($"Marca: {marcaDescricao.ToUpper()} | ", bold));
        //            //doc.Add(infoHeader);

        //            doc.Add(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(1f, 100f, BaseColor.GRAY, Element.ALIGN_CENTER, -2)));
        //            doc.Add(Chunk.NEWLINE);

        //            // Informações do cliente
        //            doc.Add(new Paragraph(" Dados do Cliente", bold) { SpacingAfter = 5f });
        //            doc.Add(new Paragraph($"• Cliente: {os.Cliente.Nome}", normal));
        //            doc.Add(new Paragraph("• Equipamento", bold) { SpacingAfter = 5f });
        //            doc.Add(new Paragraph($"• Ferramenta: {os.Ferramenta} Modelo: {os.Modelo}", normal));
        //            doc.Add(new Paragraph($"• Marca: {marcaDescricao} ", normal));
        //            if (!string.IsNullOrWhiteSpace(os.Obs))
        //                doc.Add(new Paragraph($"• Observações: {os.Obs}", normal));

        //            doc.Add(Chunk.NEWLINE);
        //            doc.Add(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.5f, 100f, BaseColor.LIGHT_GRAY, Element.ALIGN_CENTER, -1)));
        //            doc.Add(Chunk.NEWLINE);

        //            // Itens / Serviços realizados
        //            if (os.ListItensOs?.Any() == true)
        //            {
        //                doc.Add(new Paragraph("🛠️ Itens / Serviços Realizados", bold) { SpacingAfter = 8f });

        //                PdfPTable table = new PdfPTable(3) { WidthPercentage = 100 };
        //                table.SetWidths(new float[] { 4, 1, 1 });

        //                // Cabeçalho
        //                table.AddCell(new PdfPCell(new Phrase("Produto", bold)) { BackgroundColor = BaseColor.LIGHT_GRAY });
        //                table.AddCell(new PdfPCell(new Phrase("Quantidade", bold)) { BackgroundColor = BaseColor.LIGHT_GRAY });
        //                // table.AddCell(new PdfPCell(new Phrase("Valor Unit", bold)) { BackgroundColor = BaseColor.LIGHT_GRAY });
        //                table.AddCell(new PdfPCell(new Phrase("Valor Total", bold)) { BackgroundColor = BaseColor.LIGHT_GRAY });

        //                foreach (var item in os.ListItensOs)
        //                {
        //                    table.AddCell(new Phrase(item.NomeProduto, normal));
        //                    table.AddCell(new Phrase(item.Quantidade.ToString(), normal));
        //                    table.AddCell(new Phrase(item.CustoTotal.ToString("C"), normal));
        //                }
        //                doc.Add(table);
        //                doc.Add(Chunk.NEWLINE);
        //            }

        //            // Totais
        //            if (os.TotalMaoObra > 0 || os.TotalOS > 0)
        //            {
        //                doc.Add(new Paragraph("💰 Valores", bold) { SpacingBefore = 10f, SpacingAfter = 5f });
        //                doc.Add(new Paragraph($"• Mão de Obra: {os.TotalMaoObra:C}", normal));
        //                doc.Add(new Paragraph($"• Total do Serviço: {os.TotalOS:C}", bold));
        //                doc.Add(Chunk.NEWLINE);
        //            }

        //            // Instruções dependendo do status
        //            if (statusDescricao.Contains("ABERTA"))
        //            {
        //                doc.Add(new Paragraph("🔍 Aguardando orçamento", bold));
        //                doc.Add(new Paragraph("Sua OS está em análise técnica. Em até 2 dias úteis enviaremos o orçamento.", normal));
        //                doc.Add(Chunk.NEWLINE);
        //            }
        //            else if (statusDescricao.Contains("ORÇAMENTO"))
        //            {
        //                doc.Add(new Paragraph("✅ Como Aprovar", bold));
        //                doc.Add(new Paragraph("Responda a esta mensagem com:", normal));
        //                doc.Add(new Paragraph("• SIM - Para autorizar o serviço\n• NÃO - Para recusar o orçamento", normal));
        //                doc.Add(new Paragraph("Validade do orçamento: 48h", normal));
        //                doc.Add(Chunk.NEWLINE);
        //            }
        //            else if (statusDescricao.Contains("CONCLUÍDO"))
        //            {
        //                doc.Add(new Paragraph("📦 Retirada", bold));
        //                doc.Add(new Paragraph("Retirar na loja das 8h às 18h (Seg-Sex) ou 8h às 12h (Sáb).", normal));
        //                doc.Add(new Paragraph("Levar esta mensagem para retirada.", normal));
        //                doc.Add(Chunk.NEWLINE);
        //            }
        //            else if (statusDescricao.Contains("ENTREGUE"))
        //            {
        //                doc.Add(new Paragraph("🛡️ Garantia", bold));
        //                doc.Add(new Paragraph("90 dias para mão de obra e peças. Cobre defeitos relacionados ao serviço.", normal));
        //                doc.Add(Chunk.NEWLINE);
        //            }

        //            // Contato
        //            doc.Add(new Paragraph("📞 Contato", bold));
        //            doc.Add(new Paragraph("(62) 3258-4646 | (62) 3289-0694", normal));

        //            // Assinatura
        //            doc.Add(Chunk.NEWLINE);
        //            doc.Add(new Paragraph("Agradecemos pela confiança!", bold));
        //            doc.Add(new Paragraph("Equipe Forte Máquinas", normal));

        //            doc.Close();
        //            return ms.ToArray();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Erro ao gerar PDF: {ex.Message}");
        //        return null;
        //    }
        //}


        #region
        private byte[] GerarPdfDaOs(EditarOsModel os)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    Document doc = new Document(PageSize.A4, 40, 40, 30, 30);
                    PdfWriter writer = PdfWriter.GetInstance(doc, ms);
                    doc.Open();

                    var bold = new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD);
                    var normal = new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL);
                    var titleFont = new Font(Font.FontFamily.HELVETICA, 14, Font.BOLD);

                    // CABEÇALHO COMPLETO
                    PdfPTable headerTable = new PdfPTable(1) { WidthPercentage = 100 };
                    PdfPCell cellHeader = new PdfPCell { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER };
                    cellHeader.AddElement(new Paragraph("FORTE MÁQUINAS FERRAMENTAS ELÉTRICAS!", titleFont));
                    cellHeader.AddElement(new Paragraph("CNPJ: 13.436.516/0001-25", normal));
                    cellHeader.AddElement(new Paragraph("Rua Presidente Rodrigues Alves Nº 1.355 - Jardim Presidente - Goiânia GO", normal));
                    cellHeader.AddElement(new Paragraph("Fones: (62) 3258-4646 / (62) 3289-0694 / (62) 3288-7174", normal));
                    cellHeader.AddElement(new Paragraph("Email: fortemaquinas1@hotmail.com", normal));
                    headerTable.AddCell(cellHeader);
                    doc.Add(headerTable);

                    doc.Add(Chunk.NEWLINE);

                    // OS, DATAS, STATUS
                    PdfPTable osTable = new PdfPTable(3) { WidthPercentage = 100 };
                    osTable.SetWidths(new float[] { 1f, 1f, 1f });
                    osTable.AddCell(new PdfPCell(new Phrase($"Nº OS: {os.IdOs}", bold)) { Border = Rectangle.NO_BORDER });
                    osTable.AddCell(new PdfPCell(new Phrase($"Entrada: {os.DataEntrada:dd/MM/yyyy HH:mm}", bold)) { Border = Rectangle.NO_BORDER });
                    osTable.AddCell(new PdfPCell(new Phrase($"Status: {_codigosDal.GetByIdAsync(os.Status).Result?.Descricao ?? "N/D"}", bold)) { Border = Rectangle.NO_BORDER });
                    doc.Add(osTable);

                    doc.Add(Chunk.NEWLINE);

                    // CLIENTE
                    doc.Add(new Paragraph("Cliente:", bold));
                    doc.Add(new Paragraph($"Nome: {os.Cliente.Nome}", normal));
                    doc.Add(new Paragraph($"Celular: {os.Cliente.Celular}", normal));
                    //doc.Add(new Paragraph($"CPF: {os.Cliente.Cpf}   CNPJ: {os.Cliente.Cnpj}", normal));

                    doc.Add(Chunk.NEWLINE);

                    // EQUIPAMENTO
                    doc.Add(new Paragraph("Equipamento:", bold));
                    doc.Add(new Paragraph($"Ferramenta: {os.Ferramenta}", normal));
                    doc.Add(new Paragraph($"Marca: {_codigosDal.GetByIdAsync(os.Marca).Result?.Descricao ?? "N/D"}", normal));
                    doc.Add(new Paragraph($"Modelo: {os.Modelo}", normal));
                    doc.Add(new Paragraph($"Observações: {os.Obs}", normal));

                    doc.Add(Chunk.NEWLINE);

                    // DETALHES
                    doc.Add(new Paragraph("Detalhes:", bold));
                    //doc.Add(new Paragraph($"Atendente: {os.Atendente}", normal));
                    doc.Add(new Paragraph($"Técnico: {os.Tecnico}", normal));
                    //doc.Add(new Paragraph($"Responsável Orçamento: {os.ResponsavelOrcamento}", normal));
                   // doc.Add(new Paragraph($"Garantia: {(os.TemGarantia ? "SIM" : "NÃO")}", normal));
                    doc.Add(new Paragraph($"Box: {os.Box}", normal));

                    doc.Add(Chunk.NEWLINE);

                    // TABELA DE ITENS
                    if (os.ListItensOs?.Any() == true)
                    {
                        PdfPTable itensTable = new PdfPTable(6) { WidthPercentage = 100 };
                        itensTable.SetWidths(new float[] { 3, 2, 1, 2, 1, 2 });
                        string[] headers = { "Nome Produto", "Código", "Qtd", "Valor Unitário", "Desc", "Total Item" };
                        foreach (var h in headers)
                            itensTable.AddCell(new PdfPCell(new Phrase(h, bold)) { BackgroundColor = BaseColor.LIGHT_GRAY });

                        foreach (var item in os.ListItensOs)
                        {
                            itensTable.AddCell(new Phrase(item.NomeProduto, normal));
                            //itensTable.AddCell(new Phrase(item.Codigo, normal));
                            itensTable.AddCell(new Phrase(item.Quantidade.ToString(), normal));
                           // itensTable.AddCell(new Phrase(item.ValorUnitario.ToString("C"), normal));
                            itensTable.AddCell(new Phrase(item.Desconto.ToString("C"), normal));
                            itensTable.AddCell(new Phrase(item.CustoTotal.ToString("C"), normal));
                        }
                        doc.Add(itensTable);
                    }

                    doc.Add(Chunk.NEWLINE);

                    // TOTAIS
                    PdfPTable totalTable = new PdfPTable(2) { WidthPercentage = 50, HorizontalAlignment = Element.ALIGN_RIGHT };
                    totalTable.SetWidths(new float[] { 1, 1 });
                    totalTable.AddCell(new PdfPCell(new Phrase("Total Produtos:", bold)) { Border = Rectangle.NO_BORDER });
                    //totalTable.AddCell(new PdfPCell(new Phrase(os.TotalProdutos.ToString("C"), normal)) { Border = Rectangle.NO_BORDER });
                    totalTable.AddCell(new PdfPCell(new Phrase("Mão de Obra:", bold)) { Border = Rectangle.NO_BORDER });
                    totalTable.AddCell(new PdfPCell(new Phrase(os.TotalMaoObra.ToString("C"), normal)) { Border = Rectangle.NO_BORDER });
                    totalTable.AddCell(new PdfPCell(new Phrase("Total OS:", bold)) { Border = Rectangle.NO_BORDER });
                    totalTable.AddCell(new PdfPCell(new Phrase(os.TotalOS.ToString("C"), bold)) { Border = Rectangle.NO_BORDER });
                    doc.Add(totalTable);

                    doc.Add(Chunk.NEWLINE);

                    // ASSINATURAS
                    doc.Add(new Paragraph("Assinatura Cliente: _______________________", normal));
                    doc.Add(new Paragraph("Assinatura Técnico: _______________________", normal));
                    doc.Add(new Paragraph("\nAgradecemos pela confiança!", bold));
                    doc.Add(new Paragraph("Equipe Forte Máquinas", normal));

                    doc.Close();
                    return ms.ToArray();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao gerar PDF: {ex.Message}");
                return null;
            }
        }

        #endregion
        //fim

        #region AdicionarDadosOS para envio de mensagens antigo

        //private void AdicionarDadosOS(StringBuilder msg, string olaMsg, string iconeStatus, int statusId)
        //{
        //    string statusDescricao = _codigosDal.GetByIdAsync(ObjectoEditar.Status).Result.Descricao;
        //    string marcaDescricao = _codigosDal.GetByIdAsync(ObjectoEditar.Marca).Result.Descricao;

        //    msg.AppendLine($"*Olá {ObjectoEditar.Cliente.Nome}, {olaMsg}* 📝");
        //    msg.AppendLine("");
        //    msg.AppendLine($"{iconeStatus} *Status - {statusDescricao}*");
        //    msg.AppendLine("");

        //    // Seção de explicação do status (apenas para OS Aberta)
        //    if (statusId == _codigosDal.GetStatusAbertaAsync().Result.Id)
        //    {
        //        msg.AppendLine("🔍 *O que significa este status?*");
        //        msg.AppendLine($"Sua OS está em análise técnica. Em até *2 dias úteis*, enviaremos o orçamento para sua aprovação.");
        //        msg.AppendLine("");

        //        msg.AppendLine("📌 *Importante:*");
        //        msg.AppendLine("• Não realize pagamentos antecipados — somente na retirada.");
        //        msg.AppendLine("• Guarde esta mensagem para referência futura.");
        //        msg.AppendLine("• Dúvidas urgentes? Chame no (62) 3258-4646.");
        //        msg.AppendLine("• Dúvidas urgentes? Chame no (62) 3289-0694.");
        //        msg.AppendLine("");
        //        msg.AppendLine("----------------------------");
        //    }

        //    msg.AppendLine($"🔧 *Número da OS - {ObjectoEditar.IdOs}:");
        //    msg.AppendLine($"• *Cliente:* {ObjectoEditar.Cliente.Nome}");
        //    msg.AppendLine($"• *Equipamento:* {ObjectoEditar.Ferramenta}");
        //    msg.AppendLine($"• *Marca/Modelo:* {marcaDescricao} {ObjectoEditar.Modelo}");
        //    msg.AppendLine($"• *Observações:* {ObjectoEditar.Obs}");
        //    msg.AppendLine("----------------------------");
        //    msg.AppendLine("");

        //    // Adiciona produtos se não for status "Aberta"
        //    if (statusId != _codigosDal.GetStatusAbertaAsync().Result.Id)
        //    {
        //        msg.AppendLine("📋 *Itens/Serviços realizados:*");
        //        msg.AppendLine("");
        //        foreach (var item in ObjectoEditar.ListItensOs)
        //        {
        //            msg.AppendLine($"▪ {item.NomeProduto}");
        //            msg.AppendLine($"  💲 Valor: {item.CustoTotal}");
        //            msg.AppendLine("----------------------------");
        //        }
        //    }
        //}

        #endregion


        private void AdicionarDadosOS(StringBuilder msg, string saudacaoComNumero, string iconeStatus)
        {
            msg.AppendLine($"*Olá {ObjectoEditar.Cliente.Nome}*");
            msg.AppendLine($"{iconeStatus} {saudacaoComNumero}");
            msg.AppendLine("");
            msg.AppendLine("📞 (62) 3258-4646 | (62) 3289-0694");
            msg.AppendLine("📎 Segue em anexo o PDF com mais informações.");
        }


        protected override async void ApresentarDialogSucesso(string text)
        {

            #region ApresentarDialogSucesso para Envio de  mensagens antigo

            //StringBuilder msg = new();

            //if (ObjectoEditar.Status == _codigosDal.GetStatusAbertaAsync().Result.Id)
            //{
            //    AdicionarDadosOS(msg, "sua máquina foi recebida e a Ordem de Serviço foi registrada com sucesso!", "", ObjectoEditar.Status);
            //    msg.AppendLine("Agradecemos pela confiança em nosso trabalho! Estamos à disposição. 😊");
            //    msg.AppendLine("*Equipe Forte Máquinas*");
            //}
            //else if (ObjectoEditar.Status == _codigosDal.GetStatusOrçamentoAsync().Result.Id)
            //{
            //    AdicionarDadosOS(msg, "seu orçamento está pronto e aguardando sua aprovação!", "💰", ObjectoEditar.Status);

            //    msg.AppendLine("💵 *Valores do serviço:*");
            //    msg.AppendLine("");
            //    msg.AppendLine($"• *Mão de obra:* {ObjectoEditar.TotalMaoObra}");
            //    msg.AppendLine("----------------------------");
            //    msg.AppendLine($"• *Total do serviço:* {ObjectoEditar.TotalOS}");
            //    msg.AppendLine("");
            //    msg.AppendLine("✅ *Como aprovar:*");
            //    msg.AppendLine("Responda esta mensagem com:");
            //    msg.AppendLine("*SIM* - Para autorizar o serviço");
            //    msg.AppendLine("*NÃO* - Para recusar o orçamento");
            //    msg.AppendLine("");
            //    msg.AppendLine("⏳ *Validade do orçamento: 48 horas*");
            //    msg.AppendLine("");
            //    msg.AppendLine("📞 *Dúvidas?* Fale conosco:");
            //    msg.AppendLine("(62) 3258-4646 | (62) 3289-0694");
            //    msg.AppendLine("");
            //    msg.AppendLine("Agradecemos pela preferência! 🙌");
            //}
            //else if (ObjectoEditar.Status == _codigosDal.GetStatusConsertoConcluidoAsync().Result.Id)
            //{
            //    AdicionarDadosOS(msg, "seu equipamento está pronto para retirada!", "✅", ObjectoEditar.Status);

            //    msg.AppendLine("💰 *Valores a pagar:*");
            //    msg.AppendLine("");
            //    msg.AppendLine($"• *Mão de obra:* {ObjectoEditar.TotalMaoObra}");
            //    msg.AppendLine("----------------------------");
            //    msg.AppendLine($"• *Total do serviço:* {ObjectoEditar.TotalOS}");
            //    msg.AppendLine("");
            //    msg.AppendLine("🕒 *Horário de retirada:*");
            //    msg.AppendLine("Segunda a Sexta, das 8h às 18h");
            //    msg.AppendLine("Sabado, das 8h às 12h");
            //    msg.AppendLine("");
            //    msg.AppendLine("📌 *Documentos necessários:*");
            //    msg.AppendLine("• Esta mensagem (impressa ou digital)");
            //    msg.AppendLine("");
            //    msg.AppendLine("Aguardamos sua visita! 😊");
            //}
            //else if (ObjectoEditar.Status == _codigosDal.GetStatusEntregueAsync().Result.Id)
            //{
            //    AdicionarDadosOS(msg, "confirmamos a retirada do seu equipamento!", "📦", ObjectoEditar.Status);
            //    msg.AppendLine("💵 *Valores do serviço:*");
            //    msg.AppendLine("");
            //    msg.AppendLine($"• *Mão de obra:* {ObjectoEditar.TotalMaoObra}");
            //    msg.AppendLine("----------------------------");
            //    msg.AppendLine($"• *Total do serviço:* {ObjectoEditar.TotalOS}");
            //    msg.AppendLine("🛡️ *Garantia do serviço:*");
            //    msg.AppendLine("• 90 dias para mão de obra e peças");
            //    msg.AppendLine("• Cobre defeitos relacionados ao serviço Feito");
            //    msg.AppendLine("");
            //    msg.AppendLine("Obrigado por escolher a Forte Máquinas! ❤️");
            //}
            //else if (ObjectoEditar.Status == _codigosDal.GetStatusCanceladaAsync().Result.Id ||
            //         ObjectoEditar.Status == _codigosDal.GetStatusEntregueSemConsertoAsync().Result.Id)
            //{
            //    string motivo = (ObjectoEditar.Status == _codigosDal.GetStatusCanceladaAsync().Result.Id)
            //        ? "cancelamento do serviço"
            //        : "devolução sem conserto";

            //    AdicionarDadosOS(msg, $"informamos sobre a {motivo} do seu equipamento.", "⚠️", ObjectoEditar.Status);

            //    msg.AppendLine("📌 *Motivo:*");
            //    msg.AppendLine(ObjectoEditar.Status == _codigosDal.GetStatusCanceladaAsync().Result.Id
            //        ? "• Equipamento sem viabilidade técnica"
            //        : "• Custo de reparo não compensatório");
            //    msg.AppendLine("");

            //    if (ObjectoEditar.TotalOS > 0)
            //    {
            //        msg.AppendLine($"• *Mão de obra:* {ObjectoEditar.TotalMaoObra}");
            //        msg.AppendLine("----------------------------");
            //        msg.AppendLine($"• *Total do serviço:* {ObjectoEditar.TotalOS}");
            //        msg.AppendLine("");
            //    }

            //    msg.AppendLine("📞 *Dúvidas sobre o laudo técnico?*");
            //    msg.AppendLine("(62) 3258-4646 | (62) 3289-0694");
            //    msg.AppendLine("");
            //    msg.AppendLine("Agradecemos sua compreensão. 🙏");
            //}

            #endregion


            StringBuilder msg = new();

            if (ObjectoEditar.Status == _codigosDal.GetStatusAbertaAsync().Result.Id)
            {
                string texto = $"Sua Ordem de Serviço Nº: {ObjectoEditar.IdOs} foi aberta com sucesso!";
                AdicionarDadosOS(msg, texto, "✅");
                msg.AppendLine("");
                msg.AppendLine("Agradecemos pela confiança! 😊");
            }
            else if (ObjectoEditar.Status == _codigosDal.GetStatusOrçamentoAsync().Result.Id)
            {
                string texto = $"Seu orçamento da OS Nº: {ObjectoEditar.IdOs} está pronto e aguarda aprovação!";
                AdicionarDadosOS(msg, texto, "💰");
                msg.AppendLine("");
                msg.AppendLine("*Para aprovar:* Responda com SIM.");
                msg.AppendLine("*Para recusar:* Responda com NÃO.");
                msg.AppendLine("⏳ *Validade do orçamento: 48 horas*");
            }
            else if (ObjectoEditar.Status == _codigosDal.GetStatusConsertoConcluidoAsync().Result.Id)
            {
                string texto = $"Seu equipamento da OS Nº: {ObjectoEditar.IdOs} está pronto para retirada!";
                AdicionarDadosOS(msg, texto, "✅");
                msg.AppendLine("");
                msg.AppendLine("🕒 Segunda a sexta das 8h às 18h | Sábado das 8h às 12h");
            }
            else if (ObjectoEditar.Status == _codigosDal.GetStatusEntregueAsync().Result.Id)
            {
                string texto = $"Confirmamos a retirada do seu equipamento - OS Nº: {ObjectoEditar.IdOs}.";
                AdicionarDadosOS(msg, texto, "📦");
                msg.AppendLine("");
                msg.AppendLine("🛡️ *Garantia de 90 dias* para mão de obra e peças.");
            }
            else if (ObjectoEditar.Status == _codigosDal.GetStatusCanceladaAsync().Result.Id ||
                     ObjectoEditar.Status == _codigosDal.GetStatusEntregueSemConsertoAsync().Result.Id)
            {
                string motivo = (ObjectoEditar.Status == _codigosDal.GetStatusCanceladaAsync().Result.Id)
                    ? "Cancelamento da OS"
                    : "Devolução sem conserto";

                string texto = $"{motivo} confirmado para OS Nº: {ObjectoEditar.IdOs}.";
                AdicionarDadosOS(msg, texto, "⚠️");

                msg.AppendLine("");
                msg.AppendLine("📌 Motivo:");
                msg.AppendLine(ObjectoEditar.Status == _codigosDal.GetStatusCanceladaAsync().Result.Id
                    ? "• Sem viabilidade técnica"
                    : "• Custo de reparo não compensatório");
            }


            //

            //ObjectoEditar.Cliente.CelularNum;
            //string numero = "62993606091";
            string numero = "62999687129";

            // Corrigindo o erro - atribuindo o valor a uma variável
            // string numero = ObjectoEditar.Cliente.CelularNum;

            // Removendo caracteres não numéricos (opcional)
            numero = new string(numero.Where(char.IsDigit).ToArray());



            if (!string.IsNullOrWhiteSpace(msg.ToString()))
            {
                // Primeiro mostra o diálogo de impressão
                new Views.Cadastro.DialogImprimir(this).ShowDialog();

                var resultado = MessageBox.Show($"Deseja enviar a mensagem e o PDF da OS via WhatsApp?", "Confirmar envio", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (resultado == MessageBoxResult.Yes)
                {
                    try
                    {
                        // 1. Gerar o PDF primeiro para verificar
                        byte[] pdfBytes = GerarPdfDaOs(this.ObjectoEditar);

                        if (pdfBytes == null || pdfBytes.Length == 0)
                        {
                            MessageBox.Show("Falha ao gerar o PDF. Verifique os logs.");
                            return;
                        }

                        // Salvar localmente para teste (opcional)
                        // File.WriteAllBytes($"OS_{ObjectoEditar.IdOs}_teste.pdf", pdfBytes);

                        // 2. Enviar mensagem de texto
                        bool textoEnviado = await WppApi.EnviarMensagem("55" + numero, msg.ToString());

                        if (!textoEnviado)
                        {
                            MessageBox.Show("Falha ao enviar mensagem de texto. O PDF não será enviado.");
                            return;
                        }

                        // 3. Enviar o PDF
                        string nomeArquivo = $"OS_{ObjectoEditar.IdOs}_{DateTime.Now:yyyyMMdd}.pdf";
                        var (pdfEnviado, mensagemApi) = await WppApi.EnviarDocumentoAsync("55" + numero, pdfBytes, nomeArquivo);


                        if (pdfEnviado)
                        {
                            MessageBox.Show("Mensagem e PDF enviados com sucesso!");
                        }
                        else
                        {
                            MessageBox.Show($"O PDF não foi enviado. Motivo: {mensagemApi}");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erro inesperado: {ex.Message}");
                    }
                }
            }

            // ============= ATÉ AQUI =============

            // 3. Perguntar se deseja navegar para a tela de pesquisa OS
            var irParaPesquisa = MessageBox.Show("Hey Deseja pesquisar uma Os agora?", "Navegação", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (irParaPesquisa == MessageBoxResult.Yes)
            {
                UserControl pesquisaOsView = DI.PesquisaViews[nameof(PesquisaOrdemServicoViewModel)]();

                if (Switcher.layoutSwitcher != null)
                {
                    Switcher.layoutSwitcher.Navigate(pesquisaOsView);
                }
                else
                {

                    MessageBox.Show("Erro ao tentar navegar: Referência do layout principal não encontrada.", "Erro de Navegação", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            await InitializeAsync(null); // Isso vai recarregar as listas e status para uma nova OS
        }

        private async Task InitializeAsync(int? id)
        {
            if (_codigosDal == null) return; // Proteção adicional
            // Carrega a lista de marcas de ferramentas
            MarcasList = (await _codigosDal.GetListaMarcasFerramentaAsync()).ToDictionary(b => b.Id, a => a.Nome);

            // Define se é uma nova ordem de serviço ou uma já existente
            NovaOrdemServico = ObjectoEditar.Status == 0;

            if (NovaOrdemServico)
            {
                // Para uma nova ordem de serviço, define o status inicial
                var statusInicial = await _codigosDal.GetStatusAbertaAsync();
                ObjectoEditar.Status = statusInicial.Id;
                StatusList = new Dictionary<int, string>() { { statusInicial.Id, statusInicial.Nome } };
            }
            else
            {
                // Para uma ordem de serviço existente, verifica se pode editar e carrega os status seguintes
                PodeEditar = await _ordemServicoRepositorio.PodeEditarAsync(ObjectoEditar.Status);
                StatusList = (await _codigosDal.ListaStatusSeguintesAsync(ObjectoEditar.Status)).ToDictionary(b => b.Id, a => a.Nome);

                // Adiciona produtos já existentes na ordem de serviço à lista de exclusão
                foreach (var item in ObjectoEditar.ListItensOs)
                {
                    ProdutoProviderItem.ListaExclusoes.Add(item.Produto.IdProduto);
                }
            }
        }

        private bool CanExecuteCancelarOs(object parameter)
        {
            return ObjectoEditar != null && ObjectoEditar.IdOs > 0 && ObjectoEditar.Status != StatusCancelado.Id;
        }

        private async Task ExecutarCancelarOsAsync(object parameter)
        {
            MessageBoxResult result = MessageBox.Show("Tem certeza que deseja cancelar esta Os?", "Cancelar Os", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                var objDb = ObjectoEditar.DevolveObjectoBD();
                objDb.Status = StatusCancelado.Id;
                _repositorio.UpdateAsync(objDb);

                MessageBox.Show("Os cancelada com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            Id = null; // obrigamos a limpar
            PodeInserir = true; // Garante que a próxima operação será de inserção
            ObjectoEditar = NovoObjectoEditar();
            RaisePropertyChanged(nameof(ObjectoEditar));
        }

        public async Task ExecutarApagarItemNaListaAsync(object tag)
        {
            var item = (ItensOrdemServicoModelObservavel)tag;
            if (item.IdItensOs > 0)
            {
                var objBD = ItensOrdemServicoModelObservavel.MapearItemOrdemServicoModel(item);
                await _itensOsDal.DeleteItemlAsync(objBD);
            }
            ObjectoEditar.RemoverDaLista(item);
            ProdutoProviderItem.ListaExclusoes.Remove(item.Produto.IdProduto);
        }

        private async Task AtualizarItensOsAsync(int idOs)
        {
            foreach (var item in ObjectoEditar.ListItensOs)
            {
                var objBD = ItensOrdemServicoModelObservavel.MapearItemOrdemServicoModel(item);
                objBD.IdOs = idOs;
                if (objBD.IdItensOs > 0)
                {
                    await _itensOsDal.UpdateAsync(objBD);
                }
                else
                {
                    await _itensOsDal.InsertAsync(objBD);
                }
            }
        }

        public override async Task AtualizarObjectoBDAsync()
        {
            await AtualizarItensOsAsync(ObjectoEditar.IdOs);
            await base.AtualizarObjectoBDAsync();
        }

        public override EditarOsModel NovoObjectoEditar()
        {
            var obj = base.NovoObjectoEditar();

            if (obj.ListItensOs != null && !Id.HasValue)
                obj.ListItensOs.Clear();

            // Verificação de null para _codigosDal
            if (_codigosDal != null && !Id.HasValue)
            {
                var statusAberta = _codigosDal.GetStatusAbertaAsync().Result;
                obj.Status = statusAberta.Id;

                StatusList = new Dictionary<int, string>() { { statusAberta.Id, statusAberta.Nome } };
                RaisePropertyChanged(nameof(StatusList));
            }

            ProdutoProviderItem?.ListaExclusoes?.Clear();
            return obj;
        }

        public override async Task<int> InserirObjectoBDAsync()
        {
            int idOs = await base.InserirObjectoBDAsync();
            ObjectoEditar.IdOs = idOs; // Atualiza o IdOs do ObjectoEditar
            await AtualizarItensOsAsync(idOs);
            return idOs;
        }

        public bool CanExecuteApagarItem(object parameter)
        {
            return _codigosDal.PodeApagarItemAsync(ObjectoEditar.Status).GetAwaiter().GetResult(); // Chamada síncrona
        }

        public async Task ExecutarApagarItemGridNaListaAsync(object tag)
        {
            var item = (ItensOrdemServicoModelObservavel)tag;
            if (item.IdItensOs > 0)
            {
                var objBD = ItensOrdemServicoModelObservavel.MapearItemOrdemServicoModel(item);
                await _itensOsDal.DeleteItemlAsync(objBD);
            }
            ObjectoEditar.RemoverDaLista(item);
            ProdutoProviderItem.ListaExclusoes.Remove(item.Produto.IdProduto);
        }

        public bool CanExecuteAdicionarItem(object parameter)
        {
            var objBD = ItensOrdemServicoModelObservavel.MapearItemOrdemServicoModel(ObjectoEditar.ItemOsAdicionarPlanilha);
            var result = _itemOsvalidador.Validate(objBD);
            return result.IsValid;
        }

        public async Task ExecutarGuardarItemNaListaAsync(object tag)
        {
            ProdutoProviderItem.ListaExclusoes.Add(ObjectoEditar.ItemOsAdicionarPlanilha.Produto.IdProduto);
            ObjectoEditar.AdicionarNaLista();
            await Task.CompletedTask;
        }
    }
}

