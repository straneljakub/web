/* eslint-disable prettier/prettier */
import {IsInt, IsString, IsBoolean} from 'class-validator';
export class CreateUserDTO {
  @IsInt()
  readonly id: number;

  @IsString()
  readonly name: string;

  @IsString()
  readonly description: string;

  @IsBoolean()
  readonly admin: boolean;
}
