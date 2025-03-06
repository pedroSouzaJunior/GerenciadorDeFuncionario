import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Funcionario, FuncionarioService } from '../../services/funcionario.service';

@Component({
  selector: 'app-funcionario-cadastro',
  imports: [CommonModule, FormsModule],
  templateUrl: './funcionario-cadastro.component.html',
  styleUrl: './funcionario-cadastro.component.scss'
})
export class FuncionarioCadastroComponent implements OnInit {

  @Input() gestoresDisponiveis: Funcionario[] = [];
  @Output() fechar = new EventEmitter<void>();
  @Output() funcionarioAdicionado = new EventEmitter<void>();

  funcionario: Omit<Funcionario, 'id'> = {
    nome: '',
    sobrenome: '',
    email: '',
    documento: '',
    dataNascimento: '',
    telefones: [],
    role: 'Usuario',
    senha: '',
    gestorId: null
  };

  loading = false;
  sucesso = false;
  senhaValida = false;
  erros: string[] = [];

  constructor(private funcionarioService: FuncionarioService) {}

  ngOnInit(): void {
    this.carregarGestoresDisponiveis();
  }

  private carregarGestoresDisponiveis() {
    const filtro = this.gestoresDisponiveis.filter(f => f.email !== this.funcionario.email);
    this.gestoresDisponiveis = filtro;
  }

  cadastrarFuncionario() {
    this.loading = true;
    this.sucesso = false;
    this.erros = [];
  
    if (!this.funcionario.documento || !/^\d{11}$/.test(this.funcionario.documento)) {
      this.erros.push("O Documento deve conter exatamente 11 números.");
      this.loading = false;
      return;
    }
  
    if (!this.validarSenha(this.funcionario.senha)) {
      this.erros.push("A senha deve ter pelo menos 8 caracteres, incluindo uma maiúscula, uma minúscula, um número e um caractere especial.");
      this.loading = false;
      return;
    }
  
    this.funcionarioService.criarFuncionario(this.funcionario).subscribe(
      () => {
        this.loading = false;
        this.sucesso = true;
        setTimeout(() => {
          this.sucesso = false;
          this.fechar.emit();
          this.funcionarioAdicionado.emit();
        }, 1500);
      },
      (error) => this.tratarErroCadastro(error)
    );
  }

  private tratarErroCadastro(error: any) {
    this.loading = false;
    console.error("Erro ao cadastrar funcionário", error);
    this.erros = [];
  
    if (error.status === 401) {
      this.erros.push("⚠️ Sessão expirada. Faça login novamente.");
    } else if (error.status === 403) {
      this.erros.push("⚠️ Você não tem permissão para realizar esta ação.");
    } else if (error.status === 500) {
      if (error.error && typeof error.error === 'string' && error.error.includes("Você não tem permissão para criar usuários administradores")) {
        this.erros.push("⚠️ Você não tem permissão para criar usuários administradores.");
      } else {
        this.erros.push("⚠️ Erro interno no servidor. Tente novamente mais tarde.");
      }
    } else if (error.error && error.error.errors) {
      Object.keys(error.error.errors).forEach(key => {
        this.erros.push(...error.error.errors[key]);
      });
    } else {
      this.erros.push("⚠️ Erro desconhecido ao cadastrar. Tente novamente.");
    }
  }
  
  
  validarSenha(senha: string): boolean {
    const regex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/;
    this.senhaValida = regex.test(senha);
    return this.senhaValida;
  }
  

  adicionarTelefone() {
    this.funcionario.telefones.push({ tipo: 'Celular', numero: '' });
  }

  removerTelefone(index: number) {
    this.funcionario.telefones.splice(index, 1);
  }
}
