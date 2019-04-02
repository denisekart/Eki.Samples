import { Component, OnInit } from '@angular/core';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-subsubpage',
  templateUrl: './subsubpage.component.html',
  styleUrls: ['./subsubpage.component.css']
})
export class SubsubpageComponent implements OnInit {

  data: string;
  constructor(public api: ApiService) { }

  ngOnInit() {
    this.api.getSample().subscribe(x=>{
      console.log("GOT "+x)
      this.data=JSON.stringify(x);
    })
  }

}
