import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthService } from './auth.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthInterceptorService implements HttpInterceptor {
  constructor(private authService: AuthService) {
    console.log("🔥 AuthInterceptorService foi instanciado!");
  }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    console.log("🚀 Interceptando requisição:", req.url);
    const token = this.authService.getToken();
    console.log("🔑 Token sendo enviado:", token);
    console.log("Interceptando requisição:", req.url); // 🔥 Verificar se o interceptor está ativado
    console.log("Token sendo enviado:", token);

    if (token) {
      const clonedReq = req.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`,
        },
      });
      return next.handle(clonedReq);
    }

    return next.handle(req);
  }
}