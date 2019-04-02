import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { APP_BASE_HREF, LocationStrategy } from '@angular/common';
import { Observable } from 'rxjs';
import { map,take } from 'rxjs/operators';

const endpoint="/sample"
const httpOpts={
  headers:new HttpHeaders()
}
@Injectable({
  providedIn: 'root'
})
export class ApiService {
  
  constructor(private http: HttpClient, private locationStrategy: LocationStrategy) {
    console.log(`creating httpclient for basehref=${locationStrategy.getBaseHref()} and relativeRoute=${this.getRelativeRoute()}`)
   }

   private getRelativeRoute() : string{
     return this.locationStrategy.getBaseHref()+".."+endpoint;
   }

  public getSample () : Observable<any>{
    return this.http.get<any>(this.getRelativeRoute(),httpOpts).pipe(take(1),map(x=>x))
  }
}
