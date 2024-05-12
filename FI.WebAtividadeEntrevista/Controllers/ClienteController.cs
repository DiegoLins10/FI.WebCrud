﻿using FI.AtividadeEntrevista.BLL;
using WebAtividadeEntrevista.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FI.AtividadeEntrevista.DML;
using System.Text.RegularExpressions;

namespace WebAtividadeEntrevista.Controllers
{
    public class ClienteController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Incluir()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Incluir(ClienteModel model)
        {
            BoCliente bo = new BoCliente();

            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {
                // remove todos os digitos não numericos.
                string cpf = Regex.Replace(model.CPF, @"[^\d]", "");

                // valida o cpf
                var mensagensErroCPF = bo.ProcessarValidacaoCPF(cpf);

                if (mensagensErroCPF.Any())
                {
                    Response.StatusCode = 400;
                    return Json(string.Join(Environment.NewLine, mensagensErroCPF));
                }

                model.Id = bo.Incluir(new Cliente()
                {
                    CEP = model.CEP,
                    Cidade = model.Cidade,
                    Email = model.Email,
                    Estado = model.Estado,
                    Logradouro = model.Logradouro,
                    CPF = cpf,
                    Nacionalidade = model.Nacionalidade,
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                    Telefone = model.Telefone
                });

                return Json("Cadastro efetuado com sucesso");
            }
        }

        [HttpPost]
        public JsonResult Alterar(ClienteModel model)
        {
            BoCliente bo = new BoCliente();

            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {
                var clienteBase = bo.Consultar(model.Id);

                // remove todos os digitos não numericos.
                string cpf = Regex.Replace(model.CPF, @"[^\d]", "");

                // valida o cpf, se o CPF atual for igual o da base de dados, não é necessário validar novamente
                if (!clienteBase.CPF.Equals(cpf))
                {
                    var mensagensErroCPF = bo.ProcessarValidacaoCPF(cpf);

                    if (mensagensErroCPF.Any())
                    {
                        Response.StatusCode = 400;
                        return Json(string.Join(Environment.NewLine, mensagensErroCPF));
                    }
                }

                bo.Alterar(new Cliente()
                {
                    Id = model.Id,
                    CEP = model.CEP,
                    Cidade = model.Cidade,
                    Email = model.Email,
                    Estado = model.Estado,
                    Logradouro = model.Logradouro,
                    CPF = cpf,
                    Nacionalidade = model.Nacionalidade,
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                    Telefone = model.Telefone
                });

                return Json("Cadastro alterado com sucesso");
            }
        }

        [HttpGet]
        public ActionResult Alterar(long id)
        {
            BoCliente bc = new BoCliente();
            BoBeneficiario bb = new BoBeneficiario();
            Cliente cliente = bc.Consultar(id);
            List<Beneficiario> beneficiarios = bb.Consultar(id);
            Models.ClienteModel model = null;
            List<BeneficiarioModel> BeneficiariosListModel = new List<BeneficiarioModel>();

            if (beneficiarios.Any())
            {
                foreach (var item in beneficiarios)
                {
                    BeneficiarioModel beneficiarioModel = new BeneficiarioModel()
                    {
                        Id = item.Id,
                        Nome = item.Nome,
                        CPF = item.CPF,
                        IdCliente = item.IdCliente
                    };

                    BeneficiariosListModel.Add(beneficiarioModel);
                }
            }

            if (cliente != null)
            {
                model = new ClienteModel()
                {
                    Id = cliente.Id,
                    CEP = cliente.CEP,
                    Cidade = cliente.Cidade,
                    Email = cliente.Email,
                    Estado = cliente.Estado,
                    CPF = cliente.CPF,
                    Logradouro = cliente.Logradouro,
                    Nacionalidade = cliente.Nacionalidade,
                    Nome = cliente.Nome,
                    Sobrenome = cliente.Sobrenome,
                    Telefone = cliente.Telefone,
                    Beneficiarios = BeneficiariosListModel
                };
            }

            return View(model);
        }

        [HttpPost]
        public JsonResult ClienteList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                int qtd = 0;
                string campo = string.Empty;
                string crescente = string.Empty;
                string[] array = jtSorting.Split(' ');

                if (array.Length > 0)
                    campo = array[0];

                if (array.Length > 1)
                    crescente = array[1];

                List<Cliente> clientes = new BoCliente().Pesquisa(jtStartIndex, jtPageSize, campo, crescente.Equals("ASC", StringComparison.InvariantCultureIgnoreCase), out qtd);

                //Return result to jTable
                return Json(new { Result = "OK", Records = clientes, TotalRecordCount = qtd });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
    }
}