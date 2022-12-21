import { Injectable } from '@nestjs/common';
import { CreateUserDTO } from './dto/create-dto.dto';

interface User {
  readonly id: number;
  readonly name: string;
  readonly description: string;
  readonly admin: boolean;
}

@Injectable()
export class AppService {
  private users: User[] = [
    {
      id: 0,
      name: 'John Doe',
      description: 'Our admin',
      admin: true,
    },
  ];

  async addUser(createUserDTO: CreateUserDTO): Promise<any> {
    this.users.push(createUserDTO);
    return this.users.at(-1);
  }

  async getUser(userId: number): Promise<any> {
    const user = this.users.find((user) => user.id === userId);
    return user;
  }

  async getUsers(): Promise<User[]> {
    return this.users;
  }

  async editUser(id: number, createUserDTO: CreateUserDTO): Promise<any> {
    this.deleteUser(id);
    this.users.push(createUserDTO);
    return this.users.at(-1);
  }

  async deleteUser(id: number): Promise<any> {
    const userIndex = this.users.findIndex((user) => user.id === id);
    return this.users.splice(userIndex, 1);
  }
}
