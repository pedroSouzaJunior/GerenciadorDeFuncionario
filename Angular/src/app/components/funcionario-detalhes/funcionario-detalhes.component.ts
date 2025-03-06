import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule, NgIf } from '@angular/common';
import { Funcionario } from '../../services/funcionario.service';

@Component({
  selector: 'app-funcionario-detalhes',
  standalone: true,
  imports: [CommonModule, NgIf],
  templateUrl: './funcionario-detalhes.component.html',
  styleUrl: './funcionario-detalhes.component.scss'
})
export class FuncionarioDetalhesComponent {
  @Input() funcionario!: Funcionario;
  @Input() gestoresDisponiveis: Funcionario[] = [];
  @Input() isOpen = false;
  @Output() fechar = new EventEmitter<void>();

  fecharModal() {
    this.isOpen = false;
    this.fechar.emit();
  }

  get nomeGestor(): string | null {
    console.log('this.funcionario.gestorId', this.funcionario.gestorId)
    console.log('gestoresDisponiveis', this.gestoresDisponiveis)
    if (!this.funcionario.gestorId) return null;
    const gestor = this.gestoresDisponiveis.find(f => f.id === this.funcionario.gestorId);
    return gestor ? `${gestor.nome}` : ' - ';
  }
}
