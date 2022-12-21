import {
  Controller,
  Get,
  HttpStatus,
  Res,
  Body,
  Param,
  Query,
  NotFoundException,
  Put,
  Delete,
  Post,
  ParseIntPipe,
} from '@nestjs/common';
import { AppService } from './app.service';
import { CreateUserDTO } from './dto/create-dto.dto';
import { ValidationPipe } from './validation/validation.pipe';

@Controller()
export class AppController {
  constructor(private readonly appService: AppService) {}

  @Post('/')
  async create(
    @Res() res,
    @Body(new ValidationPipe()) createUserDTO: CreateUserDTO,
  ) {
    const newUser = await this.appService.addUser(createUserDTO);
    return res.status(HttpStatus.OK).json({
      message: 'User added!',
      user: newUser,
    });
  }

  @Get('/:id')
  async getUser(@Res() res, @Param('id', ParseIntPipe) id: number) {
    const user = await this.appService.getUser(id);
    if (!user) {
      throw new NotFoundException('User does not exist!');
    } else {
      return res.status(HttpStatus.OK).json(user);
    }
  }

  @Get('/')
  async getUsers(@Res() res) {
    const users = await this.appService.getUsers();
    return res.status(HttpStatus.OK).json(users);
  }

  @Put('/')
  async editUser(
    @Res() res,
    @Query('userId') id,
    @Body(new ValidationPipe()) createUserDTO: CreateUserDTO,
  ) {
    const edited = await this.appService.editUser(id, createUserDTO);
    if (!edited) {
      throw new NotFoundException('User does not exist!');
    } else {
      return res.status(HttpStatus.OK).json({
        message: 'User updated',
        user: edited,
      });
    }
  }

  @Delete('/delete')
  async deleteUser(@Res() res, @Query('userId', ParseIntPipe) id: number) {
    const deleted = await this.appService.deleteUser(id);
    if (!deleted) {
      throw new NotFoundException('User does not exist!');
    } else {
      return res.status(HttpStatus.OK).json({
        message: 'User deleted!',
        user: deleted,
      });
    }
  }
}
