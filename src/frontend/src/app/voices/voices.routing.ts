import { Routes, RouterModule } from '@angular/router';
import { VoicesComponent } from './voices.component';

const routes: Routes = [
  { path: '', pathMatch: 'full', component: VoicesComponent },
  { path: '**', redirectTo: '' }
];

export const VoicesRoutes = RouterModule.forChild(routes);
