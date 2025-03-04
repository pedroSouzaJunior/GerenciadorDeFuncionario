import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'http://localhost:5000/api/auth'; // Ajuste a URL conforme necessário

  constructor(private http: HttpClient) {}

  login(email: string, senha: string): Observable<any> {
    return this.http.post<{ token: string }>(`${this.apiUrl}/login`, { email, senha }).pipe(
      tap((response) => {
        localStorage.setItem('token', response.token); // Armazena o token no localStorage
      })
    );
  }

  logout() {
    localStorage.removeItem('token'); // Remove o token ao fazer logout
  }

  isAuthenticated(): boolean {
    return !!localStorage.getItem('token'); // Verifica se o usuário está autenticado
  }

  getToken(): string | null {
    const token = localStorage.getItem('token');
    return token;
  }
}