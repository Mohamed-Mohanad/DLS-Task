import { Routes } from '@angular/router';
import { AuthGuard } from './core/guards/auth.guard';
import { AdminGuard } from './core/guards/admin.guard';

export const routes: Routes = [
  {
    path: 'auth',
    loadChildren: () => import('./auth/auth.module').then(m => m.AuthModule)
  },
  {
    path: '',
    canActivate: [AuthGuard],
    children: [
      {
        path: '',
        loadChildren: () => import('./product/product.module').then(m => m.ProductModule)
      },
      {
        path: 'cart',
        loadChildren: () => import('./cart/cart.module').then(m => m.CartModule)
      },
      {
        path: 'admin',
        canActivate: [AdminGuard],
        loadChildren: () => import('./admin/admin.module').then(m => m.AdminModule)
      }
    ]
  },
  {
    path: '**',
    redirectTo: 'auth/login'
  }
];
