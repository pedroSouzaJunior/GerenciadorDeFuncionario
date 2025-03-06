import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Funcionario, FuncionarioService } from '../../services/funcionario.service';
import { CommonModule, NgIf } from '@angular/common';

@Component({
  selector: 'app-funcionario-exclusao',
  imports: [CommonModule, NgIf],
  templateUrl: './funcionario-exclusao.component.html',
  styleUrl: './funcionario-exclusao.component.scss'
})
export class FuncionarioExclusaoComponent {
  @Input() funcionario!: Funcionario;
  @Output() fechar = new EventEmitter<void>();
  @Output() funcionarioExcluido = new EventEmitter<void>();

  loading = false;
  erro: string | null = null;

  constructor(private funcionarioService: FuncionarioService) {}

  excluirFuncionario() {
    this.loading = true;
    this.erro = null;

    this.funcionarioService.removerFuncionario(this.funcionario.id).subscribe(
      () => {
        this.loading = false;
        this.funcionarioExcluido.emit();
        this.fechar.emit();
      },
      (error) => {
        this.loading = false;
        console.error("Erro ao excluir funcionário", error);
        this.erro = "Erro ao excluir. Tente novamente.";
      }
    );
  }
}
