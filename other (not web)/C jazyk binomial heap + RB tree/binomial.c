#include <stdio.h>
#include <stdlib.h>



struct node{
	int key;
	int degree;
	struct node* child;
	struct node* sibling;
	struct node* parent;
};

int row = 0;
int print_space = 0;


void PrintBH(struct node* H);
struct node* new_node();
struct node* binomial_heap_minimum(struct node* H);
void binomial_link(struct node* y, struct node* z);
struct node* binomial_heap_union(struct node* H1, struct node* H2);
struct node* binomial_heap_insert(struct node* H, int k);
struct node* binomial_heap_extract_min(struct node* H, struct node* c);
struct node* binomial_heap_decrease_key(struct node* H, int oldkey, int newkey);
struct node* binomial_heap_merge(struct node* H1, struct node* H2);
struct node* reverse_list(struct node* x, struct node* H);
struct node* remove_minimum(struct node* H, struct node* x);
void find(struct node* H, int oldkey, int newkey);

int main() {
	struct node* H = new_node();
	H = binomial_heap_insert(H, 10);
	H = binomial_heap_insert(H, 1);
	H = binomial_heap_insert(H, 11);
	H = binomial_heap_insert(H, 21);
	H = binomial_heap_insert(H, 2);
	H = binomial_heap_insert(H, 12);
	H = binomial_heap_insert(H, 15);
	H = binomial_heap_insert(H, 8);

	H = binomial_heap_decrease_key(H, 21, 9);

	struct node* c = new_node();

	for (int i = 0; i < 8; i++)
		H = binomial_heap_extract_min(H, c);
	
}

int MyLength(struct node* x) {
	struct node* y = x;
	int i = 0;
	while (y != NULL) {
		i++;
		y = y->sibling;
	}
	return i;
}



void PrintBHHelp(struct node* H, int last) {
	struct node* x = H;
	if (x == NULL)
		return;
	if (print_space == 1) {
		for (int j = 0; j < row * 3; j++)
			printf(" ");
		printf("\\");
	}
	print_space = 0;
	printf("|%i|", x->key);

	if (x->child == NULL) {
		print_space = 1;
		printf("\n");
		return;
	}
	printf("-");
	if (last == 1)
		row++;

	int length = MyLength(x->child);

	struct node* y[100];

	struct node* z = x->child;
	int i = 0;
	while (z != NULL) {
		y[i] = z;
		i++;
		z = z->sibling;
	}

	for (i = length - 1; i >= 0; i--) 
		if (i == 0)
			PrintBHHelp(y[i], 1);
		else 
			PrintBHHelp(y[i], 0);
	
		
}

void PrintBH(struct node* H) {
	if (H == NULL) {
		printf("Empty heap\n");
		return;
	}
	struct node* x = H;
	int length = MyLength(x);
	struct node* y[100];
	int i = 0;
	struct node* z = x;
	while (z != NULL) {
		y[i] = z;
		i++;
		z = z->sibling;
	}

	printf("END\n");
	for (i = length - 1; i >= 0; i--) {
		printf("\n");
		PrintBHHelp(y[i], 1);
		row = 0;
		print_space = 0;
		for (int j = 0; j < 50; j++)
			printf("-");
		printf("\n");
	}
	printf("START\n");
	printf("\n");

}


struct node* new_node() {
	struct node* p = malloc(sizeof(struct node));
	p->key = 0;
	p->degree = 0;
	p->parent = NULL;
	p->sibling = NULL;
	p->child = NULL;
	return p;
}

struct node* binomial_heap_minimum(struct node* H) {
	struct node* y = new_node();
	struct node* x = new_node();
	x = H;
	int min = INT_MAX;

	while (x != NULL) {
		if (x->key < min) {
			min = x->key;
			y = x;
		}
		x = x->sibling;
	}
	return y;
}

void binomial_link(struct node* y, struct node* z) {
	y->parent = z;
	y->sibling = z->child;
	z->child = y;
	z->degree = z->degree + 1;
}

struct node* binomial_heap_union(struct node* H1, struct node* H2) {
	struct node* H = new_node();
	struct node* prev_x = new_node();
	struct node* x = new_node();
	struct node* next_x = new_node();
	H = binomial_heap_merge(H1, H2);
	if (H == NULL)
		return NULL;
	prev_x = NULL;
	x = H;
	next_x = x->sibling;

	while (next_x != NULL) {
		if ((x->degree != next_x->degree) || ((next_x->sibling != NULL) && (next_x->sibling->degree == x->degree))) {
			prev_x = x;
			x = next_x;
		}
		else {
			if (x->key <= next_x->key) {
				x->sibling = next_x->sibling;
				binomial_link(next_x, x);
			}
			else {
				if (prev_x == NULL) {
					H = next_x;
				}
				else {
					prev_x->sibling = next_x;
				}
				binomial_link(x, next_x);
				x = next_x;
			}
		}
		next_x = x->sibling;
	}
	return H;
}

struct node* binomial_heap_insert(struct node* H, int k) {
	struct node* x = new_node();
	x->key = k;
	if (H->key == NULL) {
		printf("Inserting %i\n\n", x->key);
		PrintBH(x);
		return x;
	}
		
	struct node* H_ = new_node();
	H_ = x;
	H = binomial_heap_union(H, H_);
	printf("Inserting %i\n\n", k);
	PrintBH(H);
	return H;
}

struct node* binomial_heap_extract_min(struct node* H, struct node* c) {
	struct node* y = new_node();
	struct node* x = new_node();
	x = binomial_heap_minimum(H);
	H = remove_minimum(H, x);
	y = reverse_list(x->child, H);
	H = binomial_heap_union(H, y);
	c->key = x->key;
	printf("Extracting minimal %i\n\n", x->key);
	PrintBH(H);
	return H;
}

struct node* binomial_heap_decrease_key(struct node* H, int oldkey , int newkey) {
	struct node* y = new_node();
	struct node* z = new_node();
	find(H, oldkey, newkey);
	printf("Decreasing %i to %i\n\n", oldkey, newkey);
	PrintBH(H);
	return H;
}


struct node* binomial_heap_merge(struct node* H1, struct node* H2) {
	struct node* H = new_node();
	struct node* y;
	struct node* z;
	struct node* a;
	struct node* b;
	y = H1;
	z = H2;
	if (y != NULL) {
		if (z != NULL && y->degree <= z->degree)
			H = y;
		else if (z != NULL && y->degree > z->degree)
			H = z;
		else
			H = y;
	}
	else
		H = z;
	while (y != NULL && z != NULL) {
		if (y->degree < z->degree) {
			y = y->sibling;
		}
		else if (y->degree == z->degree) {
			a = y->sibling;
			y->sibling = z;
			y = a;
		}
		else {
			b = z->sibling;
			z->sibling = y;
			z = b;
		}
	}
	return H;
}

struct node* reverse_list(struct node* x, struct node* H) {
	struct node* head = new_node();
	if (x == NULL)
		return NULL;
	if (x->sibling)
	{
		head = reverse_list(x->sibling, H);
		x->sibling->sibling = x;
		x->sibling = NULL;
		return head;
	}
	return x;
}


struct node* remove_minimum(struct node* H, struct node* x) {
	if (H == x)
		return H->sibling;
	struct node* prev = H;
	struct node* current = H->sibling;
	while (current != x) {
		prev = current;
		current = current->sibling;
	}
	prev->sibling = current->sibling;
	return H;
}

void find(struct node* H, int oldkey, int newkey) {
	if (H == NULL)
		return;
	if (H->key == oldkey) {
		H->key = newkey;
		struct node* y = H;
		struct node* z = y->parent;
		while (z != NULL && y->key < z->key) {
			int i = y->key;
			y->key = z->key;
			z->key = i;
			y = z;
			z = y->parent;
		}
		return;
	}
	struct node* x = H->sibling;
	find(x, oldkey, newkey);

	x = H->child;
	find(x, oldkey, newkey);
}

