import { Component, OnInit } from '@angular/core';
import { Funcionario, FuncionarioService } from '../../services/funcionario.service';
import { CommonModule } from '@angular/common';
import { FuncionarioDetalhesComponent } from '../../components/funcionario-detalhes/funcionario-detalhes.component';
import { FuncionarioCadastroComponent } from '../../components/funcionario-cadastro/funcionario-cadastro.component';

@Component({
  selector: 'app-funcionarios-list',
  imports: [
    CommonModule,
    FuncionarioDetalhesComponent,
    FuncionarioCadastroComponent],
  templateUrl: './funcionarios-list.component.html',
  styleUrl: './funcionarios-list.component.scss'
})
export class FuncionariosListComponent implements OnInit {
  funcionarios: Funcionario[] = [];
  loading = true;
  funcionarioSelecionado!: Funcionario;
  modalAberto = false;
  modalCadastroAberto = false;

  constructor(private funcionarioService: FuncionarioService) {}

  ngOnInit(): void {
    this.carregarFuncionarios();
  }

  carregarFuncionarios() {
    this.funcionarioService.listarFuncionarios().subscribe(
      (dados) => {
        this.funcionarios = dados.map(func => ({
          ...func,
          telefones: func.telefones.map(tel => ({
            tipo: tel.tipo || 'Desconhecido',
            numero: tel.numero
          })),
          dataNascimento: new Date(func.dataNascimento).toISOString()
        }));

        this.loading = false;
      },
      (erro) => {
        console.error('Erro ao carregar funcionários', erro);
        this.loading = false;
      }
    );
  }
  

  abrirModal(funcionario: Funcionario) {
    this.funcionarioSelecionado = funcionario;
    this.modalAberto = true;
  }

  fecharModal() {
    this.modalAberto = false;
  }

  abrirModalCadastro() {
    this.modalCadastroAberto = true;
  }

  fecharModalCadastro() {
    this.modalCadastroAberto = false;
    this.carregarFuncionarios();
  }
}