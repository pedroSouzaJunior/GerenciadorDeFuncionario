import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Funcionario } from '../../services/funcionario.service';

@Component({
  selector: 'app-funcionario-detalhes',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './funcionario-detalhes.component.html',
  styleUrl: './funcionario-detalhes.component.scss'
})
export class FuncionarioDetalhesComponent {
  @Input() funcionario!: Funcionario;
  @Input() isOpen = false;
  @Output() fechar = new EventEmitter<void>();

  fecharModal() {
    this.isOpen = false;
    this.fechar.emit();
  }
}
