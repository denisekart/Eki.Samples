import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import {HttpClientModule} from '@angular/common/http'

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { SubpageComponent } from './components/subpage/subpage.component';
import { Routes, RouterModule } from '@angular/router';
import { AnotherpageComponent } from './components/anotherpage/anotherpage.component';
import { SomepageComponent } from './components/somepage/somepage.component';
import { APP_BASE_HREF, LocationStrategy, PathLocationStrategy } from '@angular/common';
import { SubsubpageComponent } from './components/subsubpage/subsubpage.component';
import { ApiService } from './services/api.service';

const appRoutes: Routes = [
  {path: "", component: SubpageComponent},
  {path: "subpage", redirectTo:""},
  {path: "anotherpage", component: AnotherpageComponent},
  {path: "somepage", component: SomepageComponent},
  {path: "somepage/subsubpage", component: SubsubpageComponent},
  {path: "**", pathMatch: "full", redirectTo: "/subpage"}
];

@NgModule({
  declarations: [
    AppComponent,
    SubpageComponent,
    AnotherpageComponent,
    SomepageComponent,
    SubsubpageComponent
  ],
  imports: [
    RouterModule.forRoot(appRoutes),
    BrowserModule,
    AppRoutingModule,
    HttpClientModule
  ],
  providers: [
     {provide: LocationStrategy, useClass: PathLocationStrategy}
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
