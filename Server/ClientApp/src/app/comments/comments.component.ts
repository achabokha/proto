import { Component, OnInit } from "@angular/core";
import { ArrayDataSource } from "@angular/cdk/collections";
import { FlatTreeControl } from "@angular/cdk/tree";

/** Flat node with expandable and level information */
interface ExampleFlatNode {
    expandable: boolean;
    name: string;
    level: number;
    isExpanded?: boolean;
}

const TREE_DATA: ExampleFlatNode[] = [
    {
        name: "John",
        expandable: true,
        level: 0
    },
    {
        name: "Peter",
        expandable: false,
        level: 1
    },
    {
        name: "John",
        expandable: false,
        level: 1
    },
    {
        name: "Peter",
        expandable: false,
        level: 1
    },
    {
        name: "John",
        expandable: true,
        level: 0
    },
    {
        name: "Karen",
        expandable: true,
        level: 1
    },
    {
        name: "John",
        expandable: false,
        level: 2
    },
    {
        name: "John",
        expandable: false,
        level: 2
    },
    {
        name: "John",
        expandable: true,
        level: 1
    },
    {
        name: "Peter",
        expandable: false,
        level: 2
    },
    {
        name: "John",
        expandable: false,
        level: 2
    }
];

@Component({
    selector: "app-comments",
    templateUrl: "./comments.component.html",
    styleUrls: ["./comments.component.scss"]
})
export class CommentsComponent implements OnInit {
    treeControl = new FlatTreeControl<ExampleFlatNode>(node => node.level, node => node.expandable);

    dataSource = new ArrayDataSource(TREE_DATA);

    constructor() {}

    ngOnInit() {}

    hasChild = (_: number, node: ExampleFlatNode) => node.expandable;

    getParentNode(node: ExampleFlatNode) {
        const nodeIndex = TREE_DATA.indexOf(node);

        for (let i = nodeIndex - 1; i >= 0; i--) {
            if (TREE_DATA[i].level === node.level - 1) {
                return TREE_DATA[i];
            }
        }

        return null;
    }

    shouldRender(node: ExampleFlatNode) {
        const parent = this.getParentNode(node);
        return !parent || parent.isExpanded;
    }
}
