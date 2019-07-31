import { Component, OnInit} from '@angular/core';
import { HttpClient} from '@angular/common/http';
import { DataService } from '../../../services/data.service';

@Component({
    selector: 'app-cryptonews',
    templateUrl: './cryptonews.component.html',
    styleUrls: ['./cryptonews.component.css']
})
export class CryptonewsComponent implements OnInit {

    constructor(private http: HttpClient, public data: DataService) {
    }

    ngOnInit(): void {
        if (!this.data.posts) {
            this.getBlogPosts();
        }
    }

    private getBlogPosts() {
        return this.http.get('https://embily.com/wp-json/wp/v2/posts/?per_page=5').subscribe(data => {
            this.data.posts = data;
            for (let p of this.data.posts) {
                let fmlink = "https://embily.com/wp-json/wp/v2/media/" + p.featured_media;

                this.http.get(fmlink).subscribe((fdata: any) => {
                        var newData: any = [];
                        newData.id = p.id;
                        newData.source_url = fdata.media_details.sizes.thumbnail.source_url;

                        this.data.featured.push(newData);
                    });
            }
        });
    }
}
