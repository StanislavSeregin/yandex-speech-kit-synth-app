import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

export const DefaultPath = '';
export const SpeechPath = 'speech';

const routes: Routes = [
  {
    path: DefaultPath,
    pathMatch: 'full',
    redirectTo: SpeechPath
  },
  {
    path: SpeechPath,
    loadChildren: () => import('./voices/voices.module').then(m => m.VoicesModule)
  },
  {
    path: '**',
    redirectTo: DefaultPath
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
