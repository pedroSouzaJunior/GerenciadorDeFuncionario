import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Funcionario, FuncionarioService } from '../../services/funcionario.service';

@Component({
  selector: 'app-funcionario-edicao',
  imports: [CommonModule, FormsModule],
  templateUrl: './funcionario-edicao.component.html',
  styleUrl: './funcionario-edicao.component.scss'
})
export class FuncionarioEdicaoComponent implements OnInit {
  @Input() funcionario!: Funcionario;
  @Input() isOpen = false;
  @Output() fechar = new EventEmitter<void>();
  @Output() funcionarioAtualizado = new EventEmitter<void>();

  loading = false;
  sucesso = false;
  erros: string[] = [];

  constructor(private funcionarioService: FuncionarioService) {}

  ngOnInit() {
    this.funcionario.dataNascimento = this.formatarData(this.funcionario.dataNascimento);
    this.funcionario.role = this.funcionario.role || 'Usuario';
  }

  formatarData(data: string): string {
    if (!data) return '';
    const dataObj = new Date(data);
    return dataObj.toISOString().split('T')[0];
  }

  atualizarFuncionario() {
    this.loading = true;
    this.sucesso = false;
    this.erros = [];

    this.funcionarioService.atualizarFuncionario(this.funcionario.id, this.funcionario).subscribe(
      () => {
        this.loading = false;
        this.sucesso = true;
        setTimeout(() => {
          this.sucesso = false;
          this.fechar.emit();
          this.funcionarioAtualizado.emit();
        }, 1500);
      },
      (error) => {
        this.loading = false;
        console.error("Erro ao atualizar funcionário", error);

        if (error.error && error.error.errors) {
          const backendErrors = error.error.errors;
          Object.keys(backendErrors).forEach(key => {
            this.erros.push(...backendErrors[key]);
          });
        } else {
          this.erros.push("Erro desconhecido ao atualizar. Tente novamente.");
        }
      }
    );
  }
}
