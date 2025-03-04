import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';

export interface Telefone {
  tipo: string;
  numero: string;
}

export interface Funcionario {
  id: number;
  nome: string;
  sobrenome: string;
  email: string;
  documento: string;
  dataNascimento: string;
  telefones: Telefone[];
  role: string;
  senha: string;
}

@Injectable({
  providedIn: 'root'
})
export class FuncionarioService {
  private API_URL = 'http://localhost:5000/api/funcionarios';

  constructor(
    private http: HttpClient,
    private authService: AuthService
  ) {}

  private getAuthHeaders(): HttpHeaders {
    const token = this.authService.getToken();
    return new HttpHeaders({
      Authorization: `Bearer ${token}`,
      'Content-Type': 'application/json'
    });
  }

  /**
   * Obtém a lista de funcionários
   */
  listarFuncionarios(): Observable<Funcionario[]> {
    return this.http.get<Funcionario[]>(this.API_URL, { headers: this.getAuthHeaders() });
  }

  /**
   * Obtém um funcionário pelo ID
   */
  obterFuncionarioPorId(id: number): Observable<Funcionario> {
    return this.http.get<Funcionario>(`${this.API_URL}/${id}`, { headers: this.getAuthHeaders() });
  }

  /**
   * Cria um novo funcionário
   */
  criarFuncionario(funcionario: Omit<Funcionario, 'id'>): Observable<Funcionario> {
    return this.http.post<Funcionario>(this.API_URL, funcionario, { headers: this.getAuthHeaders() });
  }

  /**
   * Atualiza os dados de um funcionário existente
   */
  atualizarFuncionario(id: number, funcionario: Partial<Funcionario>): Observable<void> {
    return this.http.put<void>(`${this.API_URL}/${id}`, funcionario, { headers: this.getAuthHeaders() });
  }

  /**
   * Remove um funcionário do sistema
   */
  removerFuncionario(id: number): Observable<void> {
    return this.http.delete<void>(`${this.API_URL}/${id}`, { headers: this.getAuthHeaders() });
  }
}
